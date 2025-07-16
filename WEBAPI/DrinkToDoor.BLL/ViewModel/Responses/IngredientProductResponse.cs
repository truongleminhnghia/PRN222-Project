using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class IngredientProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public int QuantityPackage { get; set; }
        public string UnitPackage { get; set; } = string.Empty;
        public string IngredientName { get; set; } = string.Empty;
        public IngredientResponse Ingredient { get; set; }
        
    }
}
