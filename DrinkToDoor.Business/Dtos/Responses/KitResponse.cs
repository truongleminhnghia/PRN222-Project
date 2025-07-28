
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.Responses
{
    public class KitResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? TotalAmount { get; set; }
        public EnumStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<KitIngredientResponse>? KitIngredients { get; set; }
        public virtual ICollection<ImageResponse>? Images { get; set; }
    }
}