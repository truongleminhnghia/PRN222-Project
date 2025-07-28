using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class KitIngredientMapper : Profile
    {
        public KitIngredientMapper()
        {
            CreateMap<KitIngredientRequest, KitIngredient>().ReverseMap();
            CreateMap<KitIngredient, KitIngredientResponse>().ReverseMap();
        }
    }
}