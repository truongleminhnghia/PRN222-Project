
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkToDoor.Data.Entities
{
    [Table("message")]
    public class Message : BaseEntity
    {
        [Key]
        [Column("message_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("sender_id")]
        public Guid SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual User? Sender { get; set; }

        [Column("message", TypeName = "varchar(500)")]
        [Required]
        public string Content { get; set; } = string.Empty;

        [Column("readed")]
        public bool Readed { get; set; } = false;
    }
}