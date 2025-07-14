
namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class CartResponse
    {
        public Guid Id { get; set; }
        public UserResponse User { get; set; }
        public List<CartItemResponse> CartItems { get; set; }

    }
}
