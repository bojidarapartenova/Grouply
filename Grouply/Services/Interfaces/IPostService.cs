using Grouply.Models;
using Grouply.ViewModels.Post;

namespace Grouply.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(string userId, Guid groupId, string? contentText,
        IFormFile? mediaFile);
        Task<Guid?> SoftDeletePostAsync(string userId, Guid postId);
    }
}