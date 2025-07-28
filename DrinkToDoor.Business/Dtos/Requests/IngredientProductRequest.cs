
namespace DrinkToDoor.Business.Dtos.Requests
{
    public class IngredientProductRequest
    {
        public Guid IngredientId { get; set; }
        public string UnitPackage { get; set; }
        public int QuantityPackage { get; set; }
        public string Name { get; set; }
    }
}