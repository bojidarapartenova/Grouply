using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grouply.Models
{
    public class ChatMember
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public Guid ChatId { get; set; }
        [Required]
        [ForeignKey(nameof(ChatId))]
        public Chat Chat { get; set; } = null!;
    }
}