using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DrinkToDoor.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public MessageRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllAsync(Guid? messageId, Guid? senderId, Guid? receiverId, string? message, bool? readed, string? sortBy, bool isDescending)
        {
            var query = _context.Messages
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .AsQueryable();

            if (messageId.HasValue) 
            {
                query = query.Where(c => c.Id == messageId.Value);
            }

            if (senderId.HasValue) 
            {
                query = query.Where(c => c.SenderId == senderId.Value);
            }

            if (receiverId.HasValue) 
            {
                query = query.Where(c => c.ReceiverId == receiverId.Value);
            }

            if (!string.IsNullOrEmpty(message)) 
            {
                query = query.Where(c => c.Content.Contains(message));
            }

            if (readed.HasValue) 
            {
                query = query.Where(c => c.Readed == readed.Value);
            }

            query = sortBy?.ToLower() switch
            {
                "senderid" => isDescending ? query.OrderByDescending(c => c.SenderId) : query.OrderBy(c => c.SenderId),
                "receiverid" => isDescending ? query.OrderByDescending(c => c.ReceiverId) : query.OrderBy(c => c.ReceiverId),
                "content" => isDescending ? query.OrderByDescending(c => c.Content) : query.OrderBy(c => c.Content),
                "readed" => isDescending ? query.OrderByDescending(c => c.Readed) : query.OrderBy(c => c.Readed),
                _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };

            return await query.ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(Guid id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
        }

        public async Task DeleteAsync(Message message)
        {
            _context.Messages.Remove(message);
        }

    }
}
