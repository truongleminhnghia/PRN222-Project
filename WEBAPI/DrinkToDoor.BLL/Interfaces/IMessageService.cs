

using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageResponse>> GetAllAsync(Guid? messageId, Guid? senderId, Guid? receiverId, string? message, bool? readed, string? sortBy, bool isDescending);
        Task<MessageResponse?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(MessageRequest request);
        Task<bool> UpdateReadStatusAsync(Guid id, bool readed);
        Task<bool> DeleteAsync(Guid id);


    }
}
