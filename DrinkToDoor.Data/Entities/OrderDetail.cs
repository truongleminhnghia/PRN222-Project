
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkToDoor.Data.Entities
{
    [Table("order_detail")]
    public class OrderDetail : BaseEntity
    {
        [Key]
        [Column("order_detail_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("quantity", TypeName = "int")]
        [Required]
        public int Quantity { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        [Column("total_price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal TotalPrice { get; set; }

        [Column("order_id")]
        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }

        [Column("kit_product_id")]
        public Guid KitProductId { get; set; }

        [ForeignKey(nameof(KitProductId))]
        public virtual KitProduct? KitProduct { get; set; }

        [Column("ingredient_product_id")]
        public Guid IngredientProductId { get; set; }
        [ForeignKey(nameof(IngredientProductId))]
        public virtual IngredientProduct? IngredientProduct { get; set; }
    }
}