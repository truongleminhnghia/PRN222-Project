
namespace DrinkToDoor.Business.Dtos.Requests
{
    public class KitRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Guid> IngredientIds { get; set; }
    }
}