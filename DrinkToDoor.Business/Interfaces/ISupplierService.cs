
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Interfaces
{
    public interface ISupplierService
    {
        Task<Supplier> CreateSupplier(SupplierRequest request);
        Task<bool> UpdateSupplier(Guid id, SupplierRequest request);
        Task<bool> DeleteSuplier(Guid id);
        Task<SupplierResponse?> GetById(Guid id);
        Task<IEnumerable<SupplierResponse>> GetAll();
        // Task<PageResult<SupplierResponse>> GetSuppliers(string? name, string? contactPerson, int pageCurrent, int pageSize);
    }
}