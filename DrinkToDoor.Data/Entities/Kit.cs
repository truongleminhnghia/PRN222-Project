
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrinkToDoor.Data.enums;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Entities
{
    [Table("kit")]
    [Index(nameof(Status), Name = "IX_Kit_Status")]
    public class Kit : BaseEntity
    {
        [Key]
        [Column("kit_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name", TypeName = "varchar(300)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "varchar(1000)")]
        public string Description { get; set; } = string.Empty;
        [Column("total_amount", TypeName = "decimal(18,2)")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Column("status", TypeName = "varchar(300)")]
        [Required]
        [EnumDataType(typeof(EnumStatus))]
        public EnumStatus Status { get; set; }

        public virtual ICollection<KitIngredient>? KitIngredients { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<KitProduct>? KitProducts { get; set; }
    }
}