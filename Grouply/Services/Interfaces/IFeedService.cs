using Grouply.Models;

namespace Grouply.Services.Interfaces
{
    public interface IFeedService
    {
        Task<IEnumerable<Post>> GetFollowingPostsAsync(string userId);
        Task<IEnumerable<Post>> GetForYouPostsAsync();
    }
}