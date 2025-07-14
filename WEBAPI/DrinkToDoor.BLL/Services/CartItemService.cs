using AutoMapper;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;


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

        
    }
}
