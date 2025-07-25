
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.Requests
{
    public class PaymentRequest
    {
        public EnumPaymentMethod PaymentMethod { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
    }
}