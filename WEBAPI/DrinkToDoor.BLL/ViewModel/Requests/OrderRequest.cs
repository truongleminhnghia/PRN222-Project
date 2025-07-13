using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class OrderRequest
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public EnumOrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string EmailShipping { get; set; } = string.Empty;
        public string PhoneShipping { get; set; } = string.Empty;
        public string FullNameShipping { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
