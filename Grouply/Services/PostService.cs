using Grouply.Data;
using Grouply.Models;
using Grouply.Models.Enums;
using Grouply.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Grouply.Services
{
    public class PostService : IPostService
    {
        private readonly GrouplyDbContext dbContext;
        private readonly IWebHostEnvironment env;

        public PostService(GrouplyDbContext dbContext, IWebHostEnvironment env)
        {
            this.dbContext = dbContext;
            this.env = env;
        }

        public async Task<Post> CreatePostAsync(string userId, Guid groupId, string? contentText, IFormFile? mediaFile)
        {
            if (string.IsNullOrWhiteSpace(contentText) && (mediaFile == null || mediaFile.Length == 0))
            {
                throw new ArgumentException("A post must contain either text or media.");
            }

            Post post = new Post()
            {
                UserId = userId,
                GroupId = groupId,
                ContentText = contentText
            };

            if (mediaFile != null && mediaFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(mediaFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await mediaFile.CopyToAsync(stream);
                }

                post.MediaUrl = $"/uploads/{fileName}";

                var extension = Path.GetExtension(mediaFile.FileName).ToLower();
                if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                {
                    post.PostType = PostType.Video;
                }
                else
                {
                    post.PostType = PostType.Photo;
                }
            }
            else
            {
                post.PostType = PostType.Text;
            }

            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();

            return post;
        }
    }
}