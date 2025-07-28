
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

        [Required]
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column("code", TypeName = "nvarchar(100)")]
        [Required]
        public string Code { get; set; } = string.Empty;

        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        [Column("status", TypeName = "varchar(300)")]
        [Required]
        [EnumDataType(typeof(EnumStatus))]
        public EnumStatus Status { get; set; }

        [Column("price", TypeName = "decimal(12,2)")]
        [Required]
        public decimal Price { get; set; }
        [Column("cost", TypeName = "decimal(12,2)")]
        public decimal? Cost { get; set; }
        [Column("stock_qty", TypeName = "int")]
        public int StockQty { get; set; }

        [Column("origin", TypeName = "varchar(300)")]
        public string? Origin { get; set; }

        [Column("trademark", TypeName = "varchar(300)")]
        public string? Trademark { get; set; }

        [Column("default_packaging_option_id")]
        public Guid? DefaultPackagingOptionId { get; set; }

        [ForeignKey(nameof(DefaultPackagingOptionId))]
        [InverseProperty(nameof(PackagingOption.IngredientDefaults))]
        public PackagingOption? DefaultPackagingOption { get; set; }

        [Column("category_id")]
        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
        public virtual ICollection<KitIngredient>? KitIngredients { get; set; }
        public virtual ICollection<Image>? Images { get; set; }

        [InverseProperty(nameof(PackagingOption.Ingredient))]
        public ICollection<PackagingOption>? PackagingOptions { get; set; }
        public virtual ICollection<IngredientProduct>? IngredientProducts { get; set; }
    }
}