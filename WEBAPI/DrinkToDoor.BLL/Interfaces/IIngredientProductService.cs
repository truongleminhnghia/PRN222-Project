using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IIngredientProductService
    {
        Task<bool> Create(IngredientProductRequest request);
        Task<IngredientProductResponse?> GetById(Guid id);
        Task<PageResult<IngredientProductResponse>> GetIngredientProducts(
            string? name,
            Guid? ingredientId,
            int pageCurrent,
            int pageSize
        );
        Task<bool> Update(Guid id, IngredientProductRequest request);
        Task<bool> Delete(Guid id);
    }
}
