using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public SupplierRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddSupplier(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            return 1;
        }

        public async Task<bool> Delete(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            return true;
        }

        public async Task<IEnumerable<Supplier>>? FindAll(string? name, string? contactPerson)
        {
            IQueryable<Supplier> query = _context.Suppliers;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(contactPerson))
            {
                query = query.Where(s => s.ContactPerson.Contains(contactPerson));
            }
            return await query.ToListAsync();
        }

        public async Task<Supplier?> FindByEmail(string email)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Supplier?> FindById(Guid id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supplier?> FindByPhone(string phone)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Phone == phone);
        }

        public async Task<Supplier?> FindByName(string name)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<int> Update(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            return 1;
        }
    }
}