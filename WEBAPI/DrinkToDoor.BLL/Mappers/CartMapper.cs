using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;


namespace DrinkToDoor.BLL.Mappers
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<Cart, CartItemResponse>();
        }
    }
}
