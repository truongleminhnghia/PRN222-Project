
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public ImageRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            return 1;
        }

        public async Task<Image?> FindByIdAsync(Guid id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<IEnumerable<Image>> FindToListAsync(Guid? ingredientId, Guid? kitId)
        {
            IQueryable<Image> query = _context.Images;

            if (ingredientId.HasValue)
            {
                query = query.Where(i => i.IngredientId == ingredientId.Value);
            }

            if (kitId.HasValue)
            {
                query = query.Where(i => i.KitId == kitId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<int> UpdateAsync(Image image)
        {
            _context.Images.Update(image);
            return 1;
        }

        public async Task<bool> DeleteAsync(Image image)
        {
            _context.Images.Remove(image);
            return true;
        }
    }
}