
namespace DrinkToDoor.Business.Dtos.Requests
{
    public class CartRequest
    {
        public Guid UserId { get; set; }
        public Guid IngredientId { get; set; }
        public string UnitPackage { get; set; }
        public int Quantity { get; set; }
    }
}