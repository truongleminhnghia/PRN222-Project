using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class CartItemRequest
    {
        public Guid IngredientProductId { get; set; }
        public Guid CartId { get; set; }

    }
}
