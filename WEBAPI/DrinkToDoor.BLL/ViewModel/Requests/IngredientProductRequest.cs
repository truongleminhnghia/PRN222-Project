using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class IngredientProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public int QuantityPackage { get; set; }
        public string UnitPackage { get; set; } = string.Empty;
        public Guid IngredientId { get; set; }
    }
}
