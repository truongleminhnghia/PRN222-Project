
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Entities
{
    [Table("order_infor")]
    public class Order : BaseEntity
    {
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("order_date", TypeName = "datetime")]
        [Required]
        public DateTime OrderDate { get; set; }

        [Column("total_amount", TypeName = "decimal(18,2)")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Column("status", TypeName = "varchar(300)")]
        [Required]
        public EnumOrderStatus Status { get; set; }

        [Column("shipping_address", TypeName = "varchar(500)")]
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        [Column("email_shipping", TypeName = "varchar(500)")]
        public string EmailShipping { get; set; } = string.Empty;

        [Column("phone_shipping", TypeName = "varchar(20)")]
        public string PhoneShipping { get; set; } = string.Empty;

        [Column("full_name_shipping", TypeName = "varchar(200)")]
        public string FullNameShipping { get; set; } = string.Empty;

        [Column("user_id")]
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}