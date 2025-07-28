
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class CartService : ICartService
    {
        private readonly ILogger<CartService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(ILogger<CartService> logger, IUnitOfWork unitOfWork,
                            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> AddToCart(CartRequest requst)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindById(requst.UserId);
                if (userExisting == null) throw new Exception($"Người dùng không tồn tại với {requst.UserId}");
                var cartExisting = await _unitOfWork.Carts.FindByUserId(userExisting.Id);
                if (cartExisting == null)
                {
                    cartExisting = new Cart
                    {
                        UserId = userExisting.Id
                    };
                    await _unitOfWork.Carts.AddAsync(cartExisting);
                }
                var ingredientExisting = await _unitOfWork.Ingredients.FindById(requst.IngredientId);
                if (ingredientExisting == null) throw new Exception($"Nguyên liệu không tồn tại với id: {requst.IngredientId}");
                var product = new IngredientProduct
                {
                    Name = ingredientExisting.Name,
                    Price = ingredientExisting.Price,
                    TotalAmount = requst.Quantity * ingredientExisting.Price,
                    QuantityPackage = requst.Quantity,
                    UnitPackage = requst.UnitPackage,
                    IngredientId = ingredientExisting.Id
                };
                var ingredientProduct = await _unitOfWork.IngredientProducts.Create(product);
                if (ingredientProduct == null) throw new Exception("Lỗi khi tạo ingredient product");
                var cartItem = new CartItem
                {
                    CartId = cartExisting.Id,
                    IngredientProductId = ingredientProduct.Id,
                    Quantity = requst.Quantity
                };
                var newCartItem = await _unitOfWork.CartItems.CreateAsync(cartItem);
                if (newCartItem == null) throw new Exception("Lỗi khi tạo ingredient product");
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> CreateCart(Guid userId)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindById(userId);
                if (userExisting == null) throw new Exception($"Người dùng không tồn tại với {userId}");
                var cartExisting = await _unitOfWork.Carts.FindByUserId(userExisting.Id);
                if (cartExisting == null)
                {
                    cartExisting = new Cart
                    {
                        UserId = userExisting.Id
                    };
                    await _unitOfWork.Carts.AddAsync(cartExisting);
                    await _unitOfWork.SaveChangesWithTransactionAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<IEnumerable<CartResponse>> GetAll()
        {
            try
            {
                var carts = await _unitOfWork.Carts.FindAll();
                if (carts == null)
                {
                    return null;
                }
                return _mapper.Map<IEnumerable<CartResponse>>(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<CartResponse?> GetById(Guid cartId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.FindById(cartId);
                if (cart == null) throw new Exception($"Cart không tồn tại với {cartId}");
                return _mapper.Map<CartResponse>(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<CartResponse?> GetByUser(Guid userId)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindById(userId);
                if (userExisting == null) throw new Exception($"Người dùng không tồn tại với {userId}");
                var cart = await _unitOfWork.Carts.FindByUserId(userId);
                if (cart == null)
                {
                    await CreateCart(userId);
                }
                return _mapper.Map<CartResponse>(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}