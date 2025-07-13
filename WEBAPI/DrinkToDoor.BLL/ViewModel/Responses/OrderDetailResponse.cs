using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class OrderDetailResponse
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid OrderId { get; set; }
        public string KitProductName { get; set; } = string.Empty;
        public Guid? IngredientProductId { get; set; }
        public string IngredientProductName { get; set; } = string.Empty;
    }
}
