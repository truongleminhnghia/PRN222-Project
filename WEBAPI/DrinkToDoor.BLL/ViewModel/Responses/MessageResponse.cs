

namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public UserResponse Sender { get; set; }
        public UserResponse Receiver { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Readed { get; set; }
    }
}
