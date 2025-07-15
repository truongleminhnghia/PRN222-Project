
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class IngredientMapper : Profile
    {
        public IngredientMapper()
        {
            CreateMap<IngredientRequest, Ingredient>().ReverseMap();
            CreateMap<Ingredient, IngredientResponse>().ReverseMap();
        }
    }
}