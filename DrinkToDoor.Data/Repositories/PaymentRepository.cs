
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public PaymentRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            return 1;
        }

        public async Task<IEnumerable<Payment>> FindAllAsync()
        {
            return await _context.Payments.Include(p => p.User).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> FindParams(EnumPaymentMethod? method, decimal? minPrice, decimal? maxPrice,
                                                            EnumPaymentStatus? status, DateTime? fromDate, DateTime? toDate,
                                                            EnumCurrency? currency, Guid? userId)
        {
            IQueryable<Payment> query = _context.Payments.Include(p => p.User);
            if (method.HasValue)
            {
                query = query.Where(p => p.PaymentMethod == method.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }

            if (currency.HasValue)
            {
                query = query.Where(p => p.Currency == currency.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Amount >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Amount <= maxPrice.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                DateTime toDateInclusive = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(p => p.CreatedAt <= toDateInclusive);
            }

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> FindAll(EnumPaymentMethod? method,
                                                        Guid? userId,
                                                        EnumPaymentStatus? status,
                                                        DateTime? fromDate,
                                                        DateTime? toDate)
        {
            IQueryable<Payment> query = _context.Payments.Include(p => p.User);

            if (method.HasValue)
                query = query.Where(p => p.PaymentMethod == method.Value);

            if (userId.HasValue)
                query = query.Where(p => p.UserId == userId.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (fromDate != null)
                query = query.Where(p => p.PaymentDate >= fromDate.Value);

            if (toDate != null)
                query = query.Where(p => p.PaymentDate <= toDate.Value);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Payment?> FindByCode(string code)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionCode == code);
        }

        public async Task<Payment?> FindById(Guid id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Update(Payment payment)
        {
            _context.Payments.Update(payment);
            return true;
        }
    }
}