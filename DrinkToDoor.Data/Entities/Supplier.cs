
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkToDoor.Data.Entities
{
    [Table("supplier")]
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        [Required]
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        /// <summary>
        /// Người liên hệ chính
        /// </summary>
        [Column("contact_person", TypeName = "nvarchar(100)")]
        public string ContactPerson { get; set; }

        /// <summary>
        /// Số điện thoại liên hệ
        /// </summary>
        [Column("phone", TypeName = "nvarchar(20)")]
        public string Phone { get; set; }

        /// <summary>
        /// Email liên hệ
        /// </summary>
        [Column("email", TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        public virtual ICollection<Ingredient>? Ingredients { get; set; }
    }
}