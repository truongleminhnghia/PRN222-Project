
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class OrderDetailMapper : Profile
    {
        public OrderDetailMapper()
        {
            CreateMap<OrderDetailRequest, OrderDetail>().ReverseMap();
        }
    }
}