
using System.ComponentModel.DataAnnotations;

namespace DrinkToDoor.Business.Dtos.Requests
{
    public class OrderRequest
    {
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        public string EmailShipping { get; set; } = string.Empty;

        public string PhoneShipping { get; set; } = string.Empty;

        public string FullNameShipping { get; set; } = string.Empty;
        [Required]
        public Guid UserId { get; set; }

        public List<OrderDetailRequest> OrderDetailsRequest { get; set; }
    }
}