

using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Mappers
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