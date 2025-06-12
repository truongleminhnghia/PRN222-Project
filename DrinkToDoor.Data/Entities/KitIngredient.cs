
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkToDoor.Data.Entities
{
    [Table("kit_ingredient")]
    public class KitIngredient
    {
        [Key]
        [Column("kit_ingredient_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("kit_id")]
        public Guid KitId { get; set; }

        [ForeignKey(nameof(KitId))]
        public virtual Kit? Kit { get; set; }

        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }

        [ForeignKey(nameof(IngredientId))]
        public virtual Ingredient? Ingredient { get; set; }
    }
}