namespace Grouply.Infrastructure
{
    using Grouply.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Grouply.Data;

    public static class ServiceProviderExtensions
    {
        public static async Task SeedAdminAsync(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<GrouplyDbContext>();

            // Ensure Admin role exists
            bool isAdminRoleExisting = await roleManager.RoleExistsAsync("Admin");
            if (!isAdminRoleExisting)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Remove duplicate admins if any
            var duplicateAdmins = await dbContext.Users
                .Where(u => u.Email == "admin@example.com")
                .ToListAsync();

            if (duplicateAdmins.Count > 1)
            {
                // Keep the first, remove the rest
                for (int i = 1; i < duplicateAdmins.Count; i++)
                {
                    dbContext.Users.Remove(duplicateAdmins[i]);
                }
                await dbContext.SaveChangesAsync();
            }

            // Find or create admin safely
            var admin = await dbContext.Users
                .Where(u => u.Email == "admin@example.com")
                .FirstOrDefaultAsync();

            if (admin == null)
            {
                admin = new ApplicationUser { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, "Password123!");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Seed groups
            var groups = GetSeedGroups(admin.Id);
            foreach (var group in groups)
            {
                bool exists = await dbContext.Groups
                    .IgnoreQueryFilters()
                    .AnyAsync(g => g.Name == group.Name);

                if (!exists)
                {
                    dbContext.Groups.Add(group);
                }
            }

            await dbContext.SaveChangesAsync();
        }


        public static async Task SeedUsersAsync(this IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<GrouplyDbContext>();

            const string userEmail = "user@example.com";
            const string userPassword = "123456";
            const string username = "UserExample";

            var existingUser = await userManager.FindByEmailAsync(userEmail);
            ApplicationUser seedUser;

            if (existingUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = username,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, userPassword);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create seed user: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                    );
                }

                seedUser = newUser;
            }
            else
            {
                seedUser = existingUser;
            }
        }

        private static List<Group> GetSeedGroups(string publisherId)
        {
            return new List<Group>
            {
                new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "Fashion & Style Inspiration",
                    Description = "Share outfit ideas, fashion trends, and style tips. Connect with fashion lovers.",
                    ImageUrl = "https://images.unsplash.com/photo-1603189343302-e603f7add05a?w=900&auto=format&fit=crop&q=60",
                    CreatedById=publisherId
                },
                new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "Photography Enthusiasts",
                    Description = "Share your photos, learn techniques, and explore photography challenges.",
                    ImageUrl = "https://images.unsplash.com/photo-1548502499-ef49e8cf98d4?w=900&auto=format&fit=crop&q=60",
                    CreatedById=publisherId
                },
                new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "Music Lovers",
                    Description = "Discuss genres, share playlists, and discover new music with fellow enthusiasts.",
                    ImageUrl = "https://images.unsplash.com/photo-1494232410401-ad00d5433cfa?w=900&auto=format&fit=crop&q=60",
                    CreatedById=publisherId
                },
                new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "Fitness Challenges",
                    Description = "Join weekly fitness challenges, share progress, and motivate each other.",
                    ImageUrl = "https://images.unsplash.com/photo-1521805103424-d8f8430e8933?w=900&auto=format&fit=crop&q=60",
                    CreatedById=publisherId
                },
                new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "Skincare & Self-Care",
                    Description = "Discuss skincare routines, product recommendations, and self-care practices.",
                    ImageUrl = "https://images.unsplash.com/photo-1608068811588-3a67006b7489?w=900&auto=format&fit=crop&q=60",
                    CreatedById=publisherId
                }
            };
        }

        public static async Task SeedPostsAsync(this IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<GrouplyDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Get some users to assign posts
            var admin = await userManager.FindByEmailAsync("admin@example.com");
            var user = await userManager.FindByEmailAsync("user@example.com");

            if (admin == null || user == null)
                throw new InvalidOperationException("Seed users must exist before seeding posts.");

            // Get existing groups
            var groups = await dbContext.Groups.ToListAsync();

            // Avoid duplicating posts
            if (dbContext.Posts.Any())
                return;

            var posts = new List<Post>
            {
                new Post
        {
            Id = Guid.NewGuid(),
            ContentText = "Sharing my latest fitness progress – feeling great!",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            GroupId = groups.FirstOrDefault(g => g.Name.Contains("Fitness Challenges"))?.Id ?? groups[3].Id,
            UserId = user.Id
        }
            };

            await dbContext.Posts.AddRangeAsync(posts);
            await dbContext.SaveChangesAsync();
        }

    }
}

