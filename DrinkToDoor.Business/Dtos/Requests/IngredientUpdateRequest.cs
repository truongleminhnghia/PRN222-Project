using System.ComponentModel.DataAnnotations;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.Requests
{
    public class IngredientUpdateRequest
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        [EnumDataType(typeof(EnumStatus))]
        public EnumStatus? Status { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public int? StockQty { get; set; }
        public virtual ICollection<ImageResponse>? Images { get; set; }
        public ICollection<PackagingOptionResponse>? PackagingOptions { get; set; }
    }
}