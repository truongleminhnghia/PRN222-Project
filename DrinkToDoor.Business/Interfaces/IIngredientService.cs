
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IIngredientService
    {
        Task<bool> CreateAsync(IngredientRequest request);
        Task<IngredientResponse?> GetByIdAsync(Guid id);
        Task<Tuple<IEnumerable<IngredientResponse>, int>> GetAsync(string? name, int pageCurrent, int pageSize);
        Task<bool> UpdateAsync(Guid id, IngredientResponse response);
        Task<bool> DeleteAsync(Guid id);
    }
}