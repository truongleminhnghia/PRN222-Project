using AutoMapper;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;


namespace DrinkToDoor.BLL.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> CreateCartItemAsync(CartItemRequest request)
        {
            var cart = await _unitOfWork.Carts.FindById(request.CartId);
            if (cart == null)
            {
                throw new ApplicationException("Cart not found");
            }

            var ingredient = await _unitOfWork.Ingredients.FindById(request.IngredientId);
            if (ingredient == null)
            {
                throw new ApplicationException("Ingredient not found");
            }

            var ingredientProduct = new IngredientProduct
            {
                Name = ingredient.Name,
                Price = ingredient.Price,
                TotalAmount = ingredient.Price * request.Quantity,
                QuantityPackage = request.Quantity,
                UnitPackage = request.EnumPackageType.ToString(),
                IngredientId = ingredient.Id
            };
            await _unitOfWork.IngredientProducts.AddAsync(ingredientProduct);

            var cartItem = new CartItem
            {
                IngredientProductId = ingredientProduct.Id,
                CartId = request.CartId,
                Quantity = request.Quantity
            };
            var result = await _unitOfWork.CartItems.CreateAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateCartItemAsync(Guid id, int quantity)
        {
            var cartItem = await _unitOfWork.CartItems.FindByIdAsync(id);
            if (cartItem == null)
            {
                throw new ApplicationException("Cart item not found");
            }

            var ingredientProduct = await _unitOfWork.IngredientProducts.FindById(cartItem.IngredientProductId);
            ingredientProduct.QuantityPackage = quantity;
            ingredientProduct.TotalAmount = ingredientProduct.Price * quantity;

            cartItem.Quantity = quantity;
            await _unitOfWork.IngredientProducts.UpdateAsync(ingredientProduct);

            var result = await _unitOfWork.CartItems.UpdateQuantityAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();
            if (result) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteCartItemAsync(Guid id)
        {
            var cartItem = await _unitOfWork.CartItems.FindByIdAsync(id);
            if (cartItem == null)
            {
                throw new ApplicationException("Cart item not found");
            }
            await _unitOfWork.CartItems.DeleteAsync(cartItem);
            return true;
        }


    }
}
