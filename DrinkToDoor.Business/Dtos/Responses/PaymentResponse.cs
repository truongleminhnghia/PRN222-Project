
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.Responses
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public EnumPaymentStatus Status { get; set; }
        public virtual UserResponse? User { get; set; }
        public EnumCurrency Currency { get; set; }
        public string? TransactionCode { get; set; }
    }
}