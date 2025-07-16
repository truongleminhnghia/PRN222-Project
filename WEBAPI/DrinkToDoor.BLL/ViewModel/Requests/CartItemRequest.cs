using DrinkToDoor.Data.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class CartItemRequest
    {
        public Guid IngredientId { get; set; }
        public Guid UserId { get; set; }
        public EnumPackageType EnumPackageType { get; set; }
        public int Quantity { get; set; } = 1;

    }
}
