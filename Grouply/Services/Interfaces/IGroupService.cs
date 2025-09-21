namespace Grouply.Services.Interfaces
{
    public interface IGroupService
    {
        Task<bool> IsUserMemberAsync(Guid groupId, string userId);
        Task JoinGroupAsync(Guid groupId, string userId);
        Task LeaveGroupAsync(Guid groupId, string userId);
    }
}