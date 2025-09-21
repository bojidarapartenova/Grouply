using Grouply.Data;
using Grouply.Models;
using Grouply.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Grouply.Services
{
    public class GroupService : IGroupService
    {
        private readonly GrouplyDbContext dbContext;

        public GroupService(GrouplyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsUserMemberAsync(Guid groupId, string userId)
        {
            return await dbContext
            .GroupMembers
            .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
        }

        public async Task JoinGroupAsync(Guid groupId, string userId)
        {
            if (!await IsUserMemberAsync(groupId, userId))
            {
                dbContext.GroupMembers.Add(new GroupMember
                {
                    GroupId = groupId,
                    UserId = userId
                });
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task LeaveGroupAsync(Guid groupId, string userId)
        {
            var member = await dbContext
            .GroupMembers
            .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

            if (member != null)
            {
                dbContext.GroupMembers.Remove(member);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}