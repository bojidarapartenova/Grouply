using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grouply.Models
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        public Guid PostId { get; set; }
        [Required]
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
    }
}