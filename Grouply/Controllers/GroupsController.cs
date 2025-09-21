using Microsoft.AspNetCore.Mvc;
using Grouply.Models;
using Grouply.Data;
using Microsoft.EntityFrameworkCore;
using Grouply.Controllers;
using Microsoft.AspNetCore.Identity;
using Grouply.Services.Interfaces;

public class GroupsController : BaseController
{
    private readonly GrouplyDbContext dbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IGroupService groupService;

    public GroupsController(GrouplyDbContext dbContext, UserManager<ApplicationUser> userManager,
    IGroupService groupService)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.groupService = groupService;
    }

    // In GroupsController
    public IActionResult Index()
    {
        var groups = dbContext.Groups.Where(g => !g.IsDeleted).ToList();
        return View(groups);
    }

    public IActionResult Details(Guid id)
    {
        var group = dbContext.Groups
            .Include(g => g.CreatedBy)
            .Include(g => g.GroupMembers)
            .Include(g => g.Posts)
                .ThenInclude(p => p.User) // 👈 load post creator
            .Include(g => g.Posts)
                .ThenInclude(p => p.Comments)
                    .ThenInclude(c => c.User) // 👈 load comment creator
            .FirstOrDefault(g => g.Id == id && !g.IsDeleted);

        if (group == null) return NotFound();

        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> Join(Guid groupId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            await groupService.JoinGroupAsync(groupId, user.Id);
        }
        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    public async Task<IActionResult> Leave(Guid groupId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            await groupService.LeaveGroupAsync(groupId, user.Id);
        }
        return Redirect(Request.Headers["Referer"].ToString());
    }
}
