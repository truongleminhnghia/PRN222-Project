using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface ISupplierService
    {
        Task<Supplier> CreateSupplier(SupplierRequest request);
        Task<bool> UpdateSupplier(Guid id, SupplierRequest request);
        Task<bool> DeleteSuplier(Guid id);
        Task<SupplierResponse?> GetById(Guid id);
        Task<PageResult<SupplierResponse>> GetSuppliers(string? name, string? contactPerson, int pageCurrent, int pageSize);
    }
}