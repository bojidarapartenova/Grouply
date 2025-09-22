using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grouply.Models;
using Grouply.Data;
using Microsoft.AspNetCore.Authorization;
using Grouply.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Grouply.ViewModels.Home;

namespace Grouply.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFeedService feedService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IFeedService feedService, UserManager<ApplicationUser> userManager)
        {
            this.feedService = feedService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await userManager.GetUserAsync(User);
                var followingPosts = await feedService.GetFollowingPostsAsync(user.Id);
                var forYouPosts = await feedService.GetForYouPostsAsync();

                var vm = new HomeFeedViewModel
                {
                    FollowingPosts = followingPosts,
                    ForYouPosts = forYouPosts,
                    HasJoinedGroups = followingPosts.Any()
                };

                return View("Index", vm);
            }

            var popularPosts = await feedService.GetForYouPostsAsync();

            return View("Landing", popularPosts);
        }
    }
}