
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryRequest, Category>();
        }
    }
}