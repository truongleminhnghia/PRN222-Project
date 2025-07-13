using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Mappers
{
    public class IngredientProductMapper : Profile
    {
        public IngredientProductMapper()
        {
            CreateMap<IngredientProductRequest, IngredientProduct>().ReverseMap();
            CreateMap<IngredientProduct, IngredientProductResponse>().ReverseMap();
        }
    }
}
