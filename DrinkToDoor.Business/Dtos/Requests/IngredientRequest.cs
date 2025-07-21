
namespace DrinkToDoor.Business.Dtos.Requests
{
    public class IngredientRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? Cost { get; set; }
        public int StockQty { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? CategoryId { get; set; }
        public List<ImageRequest>? ImagesRequest { get; set; } = new List<ImageRequest>();
        public List<PackagingOptionRequest>? PackagingOptions { get; set; } = new List<PackagingOptionRequest>();
    }
}