
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IIngredientService
    {
        Task<bool> CreateAsync(IngredientRequest request);
        Task<IngredientResponse?> GetByIdAsync(Guid id);
        Task<Tuple<IEnumerable<IngredientResponse>, int>> GetAsync(string? keyword, string? name, Guid? categoryId, decimal? minPirce,
                                                                decimal? maxPrice, decimal? minCost, decimal? maxCost,
                                                                int? minQuantity, int? maxQuantity, EnumStatus? status,
                                                                int pageCurrent, int pageSize);
        Task<bool> UpdateAsync(Guid id, IngredientUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<IngredientResponse>> OnGetSearchIngredientsAsync(string term);
    }
}