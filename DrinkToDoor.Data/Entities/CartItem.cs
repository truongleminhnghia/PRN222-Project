
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkToDoor.Data.Entities
{
    [Table("cart_item")]
    public class CartItem : BaseEntity
    {
        [Key]
        [Column("cart_item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("ingredient_product_id")]
        public Guid? IngredientProductId { get; set; }

        [ForeignKey(nameof(IngredientProductId))]
        public IngredientProduct? IngredientProduct { get; set; }

        [Column("kit_product_id")]
        public Guid? KitProductId { get; set; }

        [ForeignKey(nameof(KitProductId))]
        public KitProduct? KitProduct { get; set; }

        [Column("cart_id")]
        [Required]
        public Guid CartId { get; set; }

        [ForeignKey(nameof(CartId))]
        public virtual Cart? Cart { get; set; }
    }
}