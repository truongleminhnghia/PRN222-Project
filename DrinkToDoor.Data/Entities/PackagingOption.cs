
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Entities
{
    [Table("packagin_options")]
    public class PackagingOption
    {
        [Key]
        [Column("packaging_option_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("ingredient_id")]
        [Required]
        public Guid IngredientId { get; set; }

        [ForeignKey(nameof(IngredientId))]
        [InverseProperty(nameof(Ingredient.PackagingOptions))]
        public Ingredient? Ingredient { get; set; }

        /// <summary>
        /// Tên gói (ví dụ: "Bịch 250 g", "Thùng 12 bịch")
        /// </summary>
        [Required]
        [Column("label", TypeName = "nvarchar(100)")]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Kích thước số (ví dụ: 250, 12)
        /// </summary>
        [Column("size", TypeName = "decimal(10,2)")]
        public decimal? Size { get; set; }

        /// <summary>
        /// Đơn vị (ví dụ: "g", "pcs")
        /// </summary>
        [Column("unit", TypeName = "nvarchar(100)")]
        public string? Unit { get; set; }

        /// <summary>
        /// Loại bao bì (ví dụ: "bịch", "thùng")
        /// </summary>
        [Column("type", TypeName = "nvarchar(300)")]
        [EnumDataType(typeof(EnumPackageType))]
        public EnumPackageType Type { get; set; }

        /// <summary>
        /// Giá bán cho mỗi gói
        /// </summary>
        [Column("price", TypeName = "decimal(12,2)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Giá nhập cho mỗi gói
        /// </summary>
        [Column("cost", TypeName = "decimal(12,2)")]
        public decimal? Cost { get; set; }

        /// <summary>
        /// Tồn kho theo gói
        /// </summary>
        [Column("stock_qty", TypeName = "int")]
        public int? StockQty { get; set; }

        [InverseProperty(nameof(Ingredient.DefaultPackagingOption))]
        public virtual ICollection<Ingredient>? IngredientDefaults { get; set; }
    }
}