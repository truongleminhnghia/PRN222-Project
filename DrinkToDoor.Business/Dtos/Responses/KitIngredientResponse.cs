namespace DrinkToDoor.Business.Dtos.Responses
{
    public class KitIngredientResponse
    {
        public Guid Id { get; set; }
        public Guid KitId { get; set; }
        public Guid IngredientId { get; set; }
        public virtual IngredientResponse? Ingredient { get; set; }
    }
}