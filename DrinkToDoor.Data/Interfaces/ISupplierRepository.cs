using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface ISupplierRepository
    {
        Task<int> AddSupplier(Supplier supplier);
        Task<Supplier?> FindById(Guid id);
        Task<IEnumerable<Supplier>>? FindAll(string? name, string? contactPerson);
        Task<Supplier?> FindByEmail(string email);
        Task<Supplier?> FindByPhone(string phone);
        Task<Supplier?> FindByName(string name);
        Task<int> Update(Supplier supplier);
        Task<bool> Delete(Supplier supplier);
    }
}