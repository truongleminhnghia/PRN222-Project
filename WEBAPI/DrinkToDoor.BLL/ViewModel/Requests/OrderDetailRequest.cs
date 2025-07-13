using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class OrderDetailRequest
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid OrderId { get; set; }
        public Guid? IngredientProductId { get; set; }
    }
}
