using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class CartResponse
    {
        public Guid Id { get; set; }
        public UserResponse User { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
    }
}
