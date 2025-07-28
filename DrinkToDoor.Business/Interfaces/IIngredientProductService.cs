
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IIngredientProductService
    {
        Task<Guid> Create(IngredientProductRequest request);
        Task<IngredientProductResponse?> GetById(Guid id);
    }
}