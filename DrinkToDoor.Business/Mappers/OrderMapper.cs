
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<OrderRequest, Order>().ReverseMap();
        }
    }
}