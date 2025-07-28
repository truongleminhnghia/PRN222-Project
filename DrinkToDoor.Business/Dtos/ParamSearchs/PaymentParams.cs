using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.ParamSearchs
{
    public class PaymentParams
    {
        public EnumPaymentMethod? PaymentMethod { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public EnumPaymentStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public EnumCurrency? Currency { get; set; }
    }
}