using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<RegisterRequest, User>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
        }
    }
}
