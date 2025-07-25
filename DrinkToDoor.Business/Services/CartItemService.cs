
using AutoMapper;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartItemService> _logger;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWork unitOfWork, ILogger<CartItemService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemResponse>> GetByIds(List<Guid> ids)
        {
            try
            {
                var cartItemResponses = new List<CartItemResponse>();
                foreach (var id in ids)
                {
                    var cartItem = await _unitOfWork.CartItems.FindByIdAsync(id);
                    if (cartItem == null) throw new Exception($"Cart Item không tồn tại với {id}");
                    var item = _mapper.Map<CartItemResponse>(cartItem);
                    cartItemResponses.Add(item);
                }
                if (cartItemResponses == null) throw new Exception("danh sách cartItem không được rỗng");
                return cartItemResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}