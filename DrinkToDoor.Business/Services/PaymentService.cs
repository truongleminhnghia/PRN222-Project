
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;

namespace DrinkToDoor.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOS _payOS;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(ILogger<PaymentService> logger, IUnitOfWork unitOfWork, PayOS payOS,
                                IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _payOS = payOS;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreatePaymentAsync(PaymentRequest request)
        {
            try
            {
                var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var orderExisting = await _unitOfWork.Orders.FindById(request.OrderId);
                if (orderExisting == null) throw new Exception($"Order không tồn tại với {request.OrderId}");
                var userExisting = await _unitOfWork.Users.FindById(request.UserId);
                if (userExisting == null) throw new Exception($"User không tồn tại với {request.UserId}");
                var payment = _mapper.Map<Payment>(request);
                payment.Status = EnumPaymentStatus.PEDING;
                payment.PaymentDate = DateTime.UtcNow;
                payment.Currency = EnumCurrency.VNĐ;
                payment.Amount = orderExisting.TotalAmount;
                payment.OrderId = orderExisting.Id;
                payment.UserId = orderExisting.UserId;
                payment.TransactionCode = orderCode.ToString();
                await _unitOfWork.Payments.CreatePayment(payment);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result > 0)
                {
                    ItemData item = new ItemData(
                            orderExisting?.OrderDetails?.FirstOrDefault()?.IngredientProduct?.Name,
                            (int)(orderExisting?.OrderDetails?.Sum(i => i.Quantity) ?? 0),
                            (int)(orderExisting?.TotalAmount ?? 0)
                    );
                    List<ItemData> items = new List<ItemData> { item };
                    var getCurrentDomain = _httpContextAccessor.HttpContext.Request;
                    var domain = $"{getCurrentDomain.Scheme}://{getCurrentDomain.Host.Value}";
                    PaymentData paymentData = new PaymentData(
                        orderCode,
                        // (int)(orderExisting?.TotalAmount ?? 0),
                        2000,
                        "Đơn hàng phục vụ học tập",
                        items,
                        $"{domain}/Payments/Failed",
                        $"{domain}/Payments/Success"
                    );
                    CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                    return createPayment.checkoutUrl;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<PaymentResponse>, int>> GetAllAsync(int pageCurrent, int pageSize)
        {
            try
            {
                var orders = await _unitOfWork.Payments.FindAllAsync();
                if (orders == null) return null;
                var total = orders.Count();
                var paged = orders.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<PaymentResponse>>(paged);
                return Tuple.Create<IEnumerable<PaymentResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<PaymentResponse>, int>> GetParams(EnumPaymentMethod? method, decimal? minPrice, decimal? maxPrice,
                                                                            EnumPaymentStatus? status, DateTime? fromDate, DateTime? toDate,
                                                                            EnumCurrency? currency, Guid? userId, int pageCurrent, int pageSize)
        {
            try
            {
                var orders = await _unitOfWork.Payments.FindParams(method, minPrice, maxPrice, status, fromDate, toDate, currency, userId);
                if (orders == null) return null;
                var total = orders.Count();
                var paged = orders.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<PaymentResponse>>(paged);
                return Tuple.Create<IEnumerable<PaymentResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> VerifyPaymentAsync(string code, string status, string orderCode, bool Paid)
        {
            try
            {
                var payment = await _unitOfWork.Payments.FindByCode(orderCode.ToString());
                if (code == "00" && status == "PAID" && Paid)
                {
                    _logger.LogInformation("Payment verified successfully: {code}", code);
                    payment.Status = EnumPaymentStatus.SUCCESS;
                    if (payment?.OrderId == null) throw new Exception("id đơn hàng không được null.");
                    var orderExisting = await _unitOfWork.Orders.FindById(payment.OrderId);
                    if (orderExisting == null) throw new Exception($"Đơn hàng không tồn tại với {payment.OrderId}.");
                    orderExisting.Status = EnumOrderStatus.WAITING_DELIVER;
                    orderExisting.StatusChangedAt = DateTime.UtcNow;
                    await _unitOfWork.Orders.UpdateAsync(orderExisting);

                    var orderDetail = await _unitOfWork.OrderDetails.GetAllAsync(orderExisting.Id, null, null);
                    foreach (var detail in orderDetail)
                    {
                        var ingredientProduct = await _unitOfWork.IngredientProducts.FindById(detail.IngredientProductId.Value);
                        if (ingredientProduct == null) continue;

                        var ingredient = await _unitOfWork.Ingredients.FindById(ingredientProduct.IngredientId);
                        if (ingredient == null) continue;
                        var matchedPackaging = ingredient.PackagingOptions?
                                                            .FirstOrDefault(p => p.Type.ToString() == ingredientProduct.UnitPackage);
                        if (matchedPackaging != null)
                        {
                            matchedPackaging.StockQty -= detail.Quantity;
                            ingredient.StockQty -= detail.Quantity;
                            if (matchedPackaging.StockQty < 0)
                                matchedPackaging.StockQty = 0;
                            await _unitOfWork.Ingredients.UpdateAsync(ingredient);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Payment failed or canceled: {code}", code);
                    payment.Status = EnumPaymentStatus.FAILED;
                    if (payment?.OrderId == null) throw new Exception("id đơn hàng không được null.");
                    var orderExisting = await _unitOfWork.Orders.FindById(payment.OrderId);
                    if (orderExisting == null) throw new Exception($"Đơn hàng không tồn tại với {payment.OrderId}.");
                    orderExisting.Status = EnumOrderStatus.PAYMENT_FAILED;
                    await _unitOfWork.Orders.UpdateAsync(orderExisting);
                }

                await _unitOfWork.Payments.Update(payment);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}