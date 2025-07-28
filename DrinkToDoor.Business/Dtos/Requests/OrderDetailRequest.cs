
using System.ComponentModel.DataAnnotations;

namespace DrinkToDoor.Business.Dtos.Requests
{
    public class OrderDetailRequest
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Guid? IngredientProductId { get; set; }
    }
}