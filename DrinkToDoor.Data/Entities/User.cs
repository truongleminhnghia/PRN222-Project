

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Entities
{
    [Table("user")]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("email", TypeName = "nvarchar(300)")]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Column("last_name", TypeName = "nvarchar(300)")]
        public string? LastName { get; set; }

        [Column("first_name", TypeName = "nvarchar(300)")]
        public string? FirstName { get; set; }

        [Column("password", TypeName = "nvarchar(100)")]
        public string? Password { get; set; }

        [Column("phone", TypeName = "nvarchar(15)")]
        public string? Phone { get; set; }

        [Column("address", TypeName = "nvarchar(300)")]
        public string? Address { get; set; }

        [Column("avatar", TypeName = "longtext")]
        public string? Avatar { get; set; }

        [Column("role_name", TypeName = "varchar(100)")]
        [EnumDataType(typeof(EnumRoleName))]
        [Required]
        public EnumRoleName RoleName { get; set; }

        [EnumDataType(typeof(EnumAccountStatus))]
        [Column("account_status", TypeName = "nvarchar(50)")]
        [Required]
        public EnumAccountStatus EnumAccountStatus { get; set; }

        [Column("gender", TypeName = "nvarchar(50)")]
        [EnumDataType(typeof(EnumGender))]
        public EnumGender? Gender { get; set; }

        [Column("date_of_birth", TypeName = "datetime")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}