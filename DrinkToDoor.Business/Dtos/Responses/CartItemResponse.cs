
namespace DrinkToDoor.Business.Dtos.Responses
{
    public class CartItemResponse
    {
        public Guid Id { get; set; }
        public IngredientProductResponse? IngredientProduct { get; set; }
        public int Quantity { get; set; } = 1;
    }
}