
using System.ComponentModel.DataAnnotations;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class PackagingOptionRequest
    {
        [Required]
        public Guid IngredientId { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal? Size { get; set; }
        public string? Unit { get; set; }
        [EnumDataType(typeof(EnumPackageType))]
        public EnumPackageType Type { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public int? StockQty { get; set; }
    }
}