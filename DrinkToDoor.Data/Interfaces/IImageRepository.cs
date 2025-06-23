
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IImageRepository
    {
        Task<int> AddAsync(Image image);
        Task<Image?> FindByIdAsync(Guid id);
        Task<IEnumerable<Image>> FindToListAsync(Guid? ingredientId, Guid? kitId);
        Task<int> UpdateAsync(Image image);
        Task<bool> DeleteAsync(Image image);
    }
}