
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Entities
{
    [Table("kit_product")]
    [Index(nameof(KitId), Name = "IX_KitProduct_KitId")]
    public class KitProduct : BaseEntity
    {
        [Key]
        [Column("kit_product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name", TypeName = "varchar(300)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        [Column("total_amout", TypeName = "decimal(18,2)")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Column("quantity", TypeName = "int")]
        [Required]
        public int Quantity { get; set; }

        [Column("kit_id")]
        [Required]
        public Guid KitId { get; set; }
        [ForeignKey(nameof(KitId))]
        public virtual Kit? Kit { get; set; }
    }
}