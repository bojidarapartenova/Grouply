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

            bool isAdminRoleExisting = await roleManager.RoleExistsAsync("Admin");
            if (!isAdminRoleExisting)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                    adminUser = newAdmin;
                }
            }

            var groups = GetSeedGroups(adminUser!.Id);
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
    }
}

