
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Entities
{
    [Table("payment")]
    [Index(nameof(PaymentMethod), nameof(Status), Name = "IX_Payment_Method_Status")]
    public class Payment : BaseEntity
    {
        [Key]
        [Column("payment_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("payment_method", TypeName = "varchar(300)")]
        [Required]
        [EnumDataType(typeof(EnumPaymentMethod))]
        public EnumPaymentMethod PaymentMethod { get; set; }

        [Column("amount", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Amount { get; set; }

        [Column("payment_date", TypeName = "datetime")]
        [Required]
        public DateTime PaymentDate { get; set; }

        [Column("status", TypeName = "varchar(200)")]
        [Required]
        [EnumDataType(typeof(EnumPaymentStatus))]
        public EnumPaymentStatus Status { get; set; }

        [Column("order_id")]
        [Required]
        public Guid OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }

        [Column("user_id")]
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}