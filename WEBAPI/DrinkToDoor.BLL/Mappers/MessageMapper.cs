using AutoMapper;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;


namespace DrinkToDoor.BLL.Mappers
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, MessageResponse>();
            CreateMap<MessageRequest, Message>();
        }
    }
}
