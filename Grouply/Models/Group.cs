using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grouply.Models
{
    public class Group
    {
        [Key]
        public
        Guid Id
        { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(150)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        [Required]
        public string CreatedById { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(CreatedById))]
        public ApplicationUser CreatedBy { get; set; } = null!;

        public virtual ICollection<GroupMember> GroupMembers { get; set; } =
        new List<GroupMember>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}