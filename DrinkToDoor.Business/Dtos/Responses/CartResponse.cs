
namespace DrinkToDoor.Business.Dtos.Responses
{
    public class CartResponse
    {
        public Guid Id { get; set; }
        public UserResponse? User { get; set; }
        public virtual ICollection<CartItemResponse>? CartItems { get; set; }
    }
}