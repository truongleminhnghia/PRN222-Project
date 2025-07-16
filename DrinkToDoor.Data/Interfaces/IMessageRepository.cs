

using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync(Guid? messageId, Guid? senderId, Guid? receiverId, string? message, bool? readed, string? sortBy, bool isDescending);
        Task<Message?> GetByIdAsync(Guid id);
        Task AddAsync(Message message);
        Task UpdateAsync(Message message);
        Task DeleteAsync(Message message);
    }
}
