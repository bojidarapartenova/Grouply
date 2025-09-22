using Grouply.Models;

namespace Grouply.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(string userId, Guid groupId, string? contentText, IFormFile? mediaFile);
    }
}