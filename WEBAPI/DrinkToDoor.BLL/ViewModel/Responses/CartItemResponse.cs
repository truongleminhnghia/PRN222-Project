

namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class CartItemResponse
    {
        public Guid Id { get; set; }
        public IngredientProductResponse? IngredientProduct { get; set; }
        public CartResponse? Cart { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }

    }
}
