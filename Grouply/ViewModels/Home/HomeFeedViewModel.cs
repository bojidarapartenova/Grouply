namespace Grouply.ViewModels.Home
{
    using Grouply.Models;
    public class HomeFeedViewModel
    {
        public IEnumerable<Post> FollowingPosts { get; set; } =
        new List<Post>();

        public IEnumerable<Post> ForYouPosts { get; set; } =
        new List<Post>();

        public bool HasJoinedGroups { get; set; }
    }
}