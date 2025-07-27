using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IKitRepository
    {
        Task<int> Create(Kit kit);
        Task<Kit?> FindById(Guid id);
        Task<Kit?> FindByName(string name);
        Task<IEnumerable<Kit>> FindAll();
        Task<IEnumerable<Kit>> FindParams(string? name, double? minPrice, double? maxPrice, EnumStatus? stauts);
        Task<bool> Update(Kit kit);
        Task<bool> Delete(Kit kit);
    }
}