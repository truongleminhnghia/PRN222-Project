

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class MessageRequest
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
