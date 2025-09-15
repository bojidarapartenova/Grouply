using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grouply.Models.Enums;

namespace Grouply.Models
{
    public class GroupMember
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public Guid GroupId { get; set; }
        [Required]
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; } = null!;

        public GroupRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}