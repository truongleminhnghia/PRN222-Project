
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkToDoor.Data.Entities
{
    [Table("ingredient_product")]
    public class IngredientProduct : BaseEntity
    {
        [Key]
        [Column("ingredient_product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name", TypeName = "varchar(300)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
        [Column("total_amount", TypeName = "decimal(18,2)")]
        [Required]
        public decimal TotalAmount { get; set; }
        [Column("quantity_package", TypeName = "int")]
        [Required]
        public int QuantityPackage { get; set; }

        [Column("unit_package", TypeName = "varchar(100)")]
        [Required]
        public string UnitPackage { get; set; } = string.Empty;

        [Column("ingredient_id")]
        [Required]
        public Guid IngredientId { get; set; }
        [ForeignKey(nameof(IngredientId))]
        public virtual Ingredient? Ingredient { get; set; }
    }
}