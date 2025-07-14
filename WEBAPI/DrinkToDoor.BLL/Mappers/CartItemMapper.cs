using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;


namespace DrinkToDoor.BLL.Mappers
{
    public class CartItemMapper : Profile
    {
        public CartItemMapper()
        {
            CreateMap<CartItem, CartItemResponse>();
        }
    }
}
