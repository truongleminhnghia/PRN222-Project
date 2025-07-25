
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
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

        public async Task<bool> VerifyPaymentAsync(WebhookType type)
        {
            try
            {
                var data = _payOS.verifyPaymentWebhookData(type);
                var payment = await _unitOfWork.Payments.FindByCode(data.orderCode.ToString());
                if (payment == null)
                {
                    _logger.LogWarning(
                                   "No transaction found for order code {OrderCode}, skipping webhook processing.",
                                   data.orderCode
                               );
                    return true;
                }
                if (data.code == "00")
                {
                    _logger.LogInformation("Payment verified successfully: {Code}", data.code);
                    payment.Status = EnumPaymentStatus.SUCCESS;
                    if (payment?.OrderId == null) throw new Exception("id đơn hàng không được null.");
                    var orderExisting = await _unitOfWork.Orders.FindById(payment.OrderId);
                    if (orderExisting == null) throw new Exception($"Đơn hàng không tồn tại với {payment.OrderId}.");
                    orderExisting.Status = EnumOrderStatus.WAITING_DELIVER;
                    await _unitOfWork.Orders.UpdateAsync(orderExisting);
                }
                else
                {
                    _logger.LogWarning("Payment failed or canceled: {Code}", data.code);
                    payment.Status = EnumPaymentStatus.FAILED;
                    if (payment?.OrderId == null) throw new Exception("id đơn hàng không được null.");
                    var orderExisting = await _unitOfWork.Orders.FindById(payment.OrderId);
                    if (orderExisting == null) throw new Exception($"Đơn hàng không tồn tại với {payment.OrderId}.");
                    orderExisting.Status = EnumOrderStatus.PAYMENT_FAILED;
                    await _unitOfWork.Orders.UpdateAsync(orderExisting);
                }

                await _unitOfWork.Payments.Update(payment);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}