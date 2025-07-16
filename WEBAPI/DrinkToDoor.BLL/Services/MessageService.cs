using AutoMapper;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace DrinkToDoor.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageResponse>> GetAllAsync(Guid? messageId, Guid? senderId, Guid? receiverId, string? message, bool? readed, string? sortBy, bool isDescending)
        {
            var messages = await _unitofWork.Messages.GetAllAsync(messageId, senderId, receiverId, message, readed, sortBy, isDescending);
            return _mapper.Map<IEnumerable<MessageResponse>>(messages);
        }

        public async Task<MessageResponse?> GetByIdAsync(Guid id)
        {
            var message = await _unitofWork.Messages.GetByIdAsync(id);
            if (message == null) throw new ApplicationException("Message not found");
            return _mapper.Map<MessageResponse>(message);
        }

        public async Task<bool> CreateAsync(MessageRequest request)
        {
            var message = _mapper.Map<Message>(request);
            var sender = await _unitofWork.Users.FindById(request.SenderId);
            if (sender == null) throw new ApplicationException("Sender not found");
            var receiver = await _unitofWork.Users.FindById(request.ReceiverId);
            if (receiver == null) throw new ApplicationException("Receiver not found");
            await _unitofWork.Messages.AddAsync(message);
            await _unitofWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReadStatusAsync(Guid id, bool readed)
        {
            var message = await _unitofWork.Messages.GetByIdAsync(id);
            if (message == null) return false;

            message.Readed = readed;
            await _unitofWork.Messages.UpdateAsync(message);
            await _unitofWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var message = await _unitofWork.Messages.GetByIdAsync(id);
            if (message == null) return false;

            await _unitofWork.Messages.DeleteAsync(message);
            await _unitofWork.SaveChangesAsync();
            return true;
        }
    }
}
