using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class KitMapper : Profile
    {
        public KitMapper()
        {
            CreateMap<KitRequest, Kit>().ReverseMap();
            CreateMap<Kit, KitResponse>().ReverseMap();
        }
    }
}