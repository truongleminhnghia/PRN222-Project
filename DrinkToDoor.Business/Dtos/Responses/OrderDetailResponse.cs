
namespace DrinkToDoor.Business.Dtos.Responses
{
    public class OrderDetailResponse
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid OrderId { get; set; }
        public Guid? KitProductId { get; set; }
        public Guid? IngredientProductId { get; set; }
        public virtual IngredientProductResponse? IngredientProduct { get; set; }
    }
}