
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Entities
{
    [Table("ingredient")]

    public class Ingredient : BaseEntity
    {
        [Key]
        [Column("ingredient_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name", TypeName = "varchar(300)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "varchar(1000)")]
        public string Description { get; set; } = string.Empty;

        [Column("price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        [Column("quantity_single", TypeName = "int")]
        public int? QuantitySingle { get; set; }

        [Column("quantity_per_carton", TypeName = "int")]
        public int? QuantityPerCarton { get; set; }

        [Column("status", TypeName = "varchar(300)")]
        [Required]
        [EnumDataType(typeof(EnumStatus))]
        public EnumStatus Status { get; set; }

        [Column("manufacturer", TypeName = "varchar(300)")]
        public string? Manufacturer { get; set; }

        [Column("weight_per_bag", TypeName = "float")]
        [Required]
        public float? WeightPerBag { get; set; }

        [Column("quantity_per_packing_unit", TypeName = "int")]
        [Required]
        public int QuantityPerPackingUnit { get; set; }

        [Column("packing_unit", TypeName = "JSON")]
        [Required]
        public string[]? PackingUnit { get; set; }

        [Column("category_id")]
        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
        public virtual ICollection<KitIngredient>? KitIngredients { get; set; }
        public virtual ICollection<Image>? Images { get; set; }

        public virtual ICollection<IngredientProduct>? IngredientProducts { get; set; }
    }
}