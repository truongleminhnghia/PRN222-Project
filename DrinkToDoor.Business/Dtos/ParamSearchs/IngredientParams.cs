
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.ParamSearchs
{
    public class IngredientParams
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinCost { get; set; }
        public decimal? MaxCost { get; set; }
        public int? MinStockQty { get; set; }
        public int? MaxStockQty { get; set; }
        public EnumStatus? Status { get; set; }
    }
}