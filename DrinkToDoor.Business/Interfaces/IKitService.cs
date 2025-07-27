using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IKitService
    {
        Task<bool> CreateKit(KitRequest request);
        Task<KitResponse?> GetById(Guid id);
        Task<Tuple<IEnumerable<KitResponse>, int>> GetAll(int pageCurrent, int pageSize);
        Task<bool> Update(Guid id, KitRequest request);
        Task<bool> Delete(Guid id);
    }
}