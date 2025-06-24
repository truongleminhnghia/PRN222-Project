using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Mappers
{
    public class PackagingOptionMapper : Profile
    {
        public PackagingOptionMapper()
        {
            CreateMap<PackagingOptionRequest, PackagingOption>().ReverseMap();
            CreateMap<PackagingOption, PackagingOptionResponse>().ReverseMap();
        }
    }
}