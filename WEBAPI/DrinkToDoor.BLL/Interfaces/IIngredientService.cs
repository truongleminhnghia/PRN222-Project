
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IIngredientService
    {
        Task<bool> CreateIngredient(IngredientRequest request);
        Task<IngredientResponse?> GetById(Guid id);
        Task<PageResult<IngredientResponse>> GetIngredients(int pageCurrent, int pageSize);
        
    }
}