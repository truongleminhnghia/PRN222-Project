
using AutoMapper;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class IngredientProductMapper : Profile
    {
        public IngredientProductMapper()
        {
            CreateMap<IngredientProduct, IngredientProductResponse>().ReverseMap();
        }
    }
}