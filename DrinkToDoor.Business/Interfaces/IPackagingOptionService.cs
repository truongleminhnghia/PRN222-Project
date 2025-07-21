
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IPackagingOptionService
    {
        Task<bool> Add(Guid id, List<PackagingOptionRequest> request);
        Task<bool> Delete(Guid id);
        // Task<bool> Update(Guid id, PackagingOptionRequest request);
        Task<PackagingOptionResponse?> GetById(Guid id);
        // Task<PageResult<PackagingOptionResponse>> GetAll(int pageCurrent, int pageSize);
    }
}