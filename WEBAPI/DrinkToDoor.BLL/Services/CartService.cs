using AutoMapper;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;



namespace DrinkToDoor.BLL.Services
{
    public class CartService : ICartService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResult<CartResponse>> GetAllWithParamsAsync(Guid? userId, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.Carts.GetAllWithParamsAsync(
                userId, sortBy, isDescending, pageNumber, pageSize);

            var pagedResult = result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var dtoList = _mapper.Map<List<CartResponse>>(pagedResult);
            if (sortBy?.ToLower() == "createdat")
            {
                foreach (var cart in dtoList)
                {
                    cart.CartItems = isDescending
                        ? cart.CartItems.OrderByDescending(i => i.CreatedAt).ToList()
                        : cart.CartItems.OrderBy(i => i.CreatedAt).ToList();
                }
            }
            return new PageResult<CartResponse>(dtoList, pageSize, pageNumber, result.Count());

        }

        public async Task<bool> CreateCartAsync(Guid userId)
        {
            var existingCart = await _unitOfWork.Carts.FindByUserId(userId);

            if (existingCart != null)
                return false;

            var newCart = new Cart
            {
                UserId = userId,
            };

            var created = await _unitOfWork.Carts.AddAsync(newCart);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
