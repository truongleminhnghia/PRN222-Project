
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public UserRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(User user)
        {
            await _context.AddAsync(user);
            return 1;
        }

        public async Task<bool> Delete(User user)
        {
            _context.Users.Remove(user);
            return true;
        }

        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> FindById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> FindToList(string? lastName, string? firstName, EnumRoleName? roleName, EnumAccountStatus? status, EnumGender? gender)
        {
            IQueryable<User> query = _context.Users;
            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(u => u.LastName != null && u.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(u => u.FirstName != null && u.FirstName.Contains(firstName));
            }
            if (gender.HasValue)
            {
                query = query.Where(u => u.Gender == gender.Value);
            }
            if (status.HasValue)
            {
                query = query.Where(u => u.EnumAccountStatus == status.Value);
            }
            if (roleName.HasValue)
            {
                query = query.Where(u => u.RoleName == roleName.Value);
            }
            query = query.OrderByDescending(a => a.CreatedAt);
            return await query.ToListAsync();
        }

        public async Task<int> Update(User user)
        {
            _context.Users.Update(user);
            return 1;
        }
    }
}