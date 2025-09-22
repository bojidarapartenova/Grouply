namespace Grouply.ViewModels.Post
{
    public class DeletePostViewModel
    {
        public Guid Id { get; set; }

        public string PublisherId { get; set; } = null!;
        public string Publisher { get; set; } = null!;
    }
}