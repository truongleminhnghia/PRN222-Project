
namespace DrinkToDoor.Business.Dtos.Responses
{
    public class IngredientProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public int QuantityPackage { get; set; }
        public string UnitPackage { get; set; } = string.Empty;
        public Guid IngredientId { get; set; }
        public virtual IngredientResponse? Ingredient { get; set; }
    }
}