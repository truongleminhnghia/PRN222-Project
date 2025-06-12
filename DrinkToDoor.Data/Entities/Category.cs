
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Entities
{
    [Table("category")]
    [Index(nameof(CategoryType), nameof(Name), Name = "IX_Category_Type_Name")]
    public class Category : BaseEntity
    {
        [Key]
        [Column("category_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name", TypeName = "varchar(100)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("category_type", TypeName = "varchar(500)")]
        [Required]
        [EnumDataType(typeof(EnumCategoryType))]
        public EnumCategoryType CategoryType { get; set; }

        public virtual ICollection<Ingredient>? Ingredients { get; set; }
        public virtual ICollection<Kit>? Kits { get; set; }
    }
}