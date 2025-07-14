
using System.ComponentModel.DataAnnotations;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.Responses
{
    public class IngredientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Description { get; set; }
        [EnumDataType(typeof(EnumStatus))]
        public EnumStatus Status { get; set; }
        public decimal Price { get; set; }
        public decimal? Cost { get; set; }
        public int StockQty { get; set; }
        public Guid? SupplierId { get; set; }
        public SupplierResponse? Supplier { get; set; }
        public PackagingOptionResponse? DefaultPackagingOption { get; set; }
        public virtual CategoryResponse? Category { get; set; }
        // public virtual ICollection<KitIngredient>? KitIngredients { get; set; }
        public virtual ICollection<ImageResponse>? Images { get; set; }
        public ICollection<PackagingOptionResponse>? PackagingOptions { get; set; }
        
    }
}