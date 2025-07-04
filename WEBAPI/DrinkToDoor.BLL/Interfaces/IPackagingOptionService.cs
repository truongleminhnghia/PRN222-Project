using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IPackagingOptionService
    {
        // Task<bool> Create(PackagingOptionRequest request);
        Task<bool> Add(Guid id, List<PackagingOptionRequest> request);
        Task<bool> Delete(Guid id);
        // Task<bool> Update(Guid id, PackagingOptionRequest request);
        Task<PackagingOptionResponse?> GetById(Guid id);
        Task<PageResult<PackagingOptionResponse>> GetAll(int pageCurrent, int pageSize);
    }
}