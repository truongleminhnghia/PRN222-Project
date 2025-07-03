
namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class IngredientRequest
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? Cost { get; set; }
        public int StockQty { get; set; }
        public SupplierRequest? Supplier { get; set; }
        public CategoryRequest? Category { get; set; }
        public List<ImageRequest>? Images { get; set; }
        public List<PackagingOptionRequest>? PackagingOptions { get; set; }
    }
}