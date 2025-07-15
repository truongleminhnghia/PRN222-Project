
using AutoMapper;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class PackageOptionMapper : Profile
    {
     public PackageOptionMapper()
     {
            CreateMap<PackagingOption, PackagingOptionResponse>().ReverseMap();
     }
    }
}