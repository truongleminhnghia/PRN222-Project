
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<Cart, CartResponse>().ReverseMap();
        }
    }
}