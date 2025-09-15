using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grouply.Models.Enums;

namespace Grouply.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public PostType PostType { get; set; }

        [MaxLength(500)]
        public string? ContentText { get; set; }

        public string? MediaUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public Guid GroupId { get; set; }
        [Required]
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; } = null!;

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}