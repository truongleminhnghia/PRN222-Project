using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Mappers
{
    public class ImageMapper : Profile
    {
        public ImageMapper()
        {
            CreateMap<Image, ImageResponse>().ReverseMap();
        }
    }
}