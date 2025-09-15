using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Grouply.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string? DisplayName { get; set; }

        [MaxLength(150)]
        public string? Bio { get; set; }

        public string ImageUrl { get; set; } = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";
        public virtual ICollection<Group> Groups { get; set; } =
        new List<Group>();
        public virtual ICollection<GroupMember> GroupMembers { get; set; } =
        new List<GroupMember>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<ChatMember> ChatMembers { get; set; } =
        new List<ChatMember>();
        public virtual ICollection<Message> Messages { get; set; } =
        new List<Message>();
    }
}