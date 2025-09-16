namespace Grouply.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedInitialDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            await serviceProvider.SeedAdminAsync();
            await serviceProvider.SeedUsersAsync();
            await serviceProvider.SeedPostsAsync();
        }
    }
}
