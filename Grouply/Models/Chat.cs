using System.ComponentModel.DataAnnotations;

namespace Grouply.Models
{
    public class Chat
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsGroupChat { get; set; } = false;

        public virtual ICollection<ChatMember> ChatMembers { get; set; } =
        new List<ChatMember>();
        public virtual ICollection<Message> Messages { get; set; } =
        new List<Message>();
    }
}