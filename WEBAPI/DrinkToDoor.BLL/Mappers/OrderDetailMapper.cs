using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Mappers
{
    public class OrderDetailMapper : Profile
    {
        public OrderDetailMapper()
        {
            CreateMap<OrderDetailRequest, OrderDetail>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
        }
    }
}
