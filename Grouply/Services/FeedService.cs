using Grouply.Data;
using Grouply.Models;
using Grouply.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Grouply.Services
{
    public class FeedService : IFeedService
    {
        private readonly GrouplyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public FeedService(GrouplyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Post>> GetFollowingPostsAsync(string userId)
        {
            return await dbContext.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Likes)
                .Where(p => p.Group.GroupMembers.Any(gm => gm.UserId == userId) && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetForYouPostsAsync()
        {
            var since = DateTime.UtcNow.AddDays(-14);

            return await dbContext.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Likes)
                .Where(p => !p.IsDeleted && p.CreatedAt >= since)
                .OrderByDescending(p => p.Likes.Count + p.Comments.Count * 2)
                .Take(30)
                .ToListAsync();
        }
    }
}