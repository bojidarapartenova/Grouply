using Microsoft.AspNetCore.Mvc;
using Grouply.Models;
using Grouply.Data;
using Microsoft.EntityFrameworkCore;
using Grouply.Controllers;
using Microsoft.AspNetCore.Identity;
using Grouply.Services.Interfaces;
using Grouply.ViewModels.Post;

public class GroupsController : BaseController
{
    private readonly GrouplyDbContext dbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IGroupService groupService;
    private readonly IPostService postService;

    public GroupsController(GrouplyDbContext dbContext, UserManager<ApplicationUser> userManager,
    IGroupService groupService, IPostService postService)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.groupService = groupService;
        this.postService = postService;
    }

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
                .ThenInclude(p => p.User)
            .Include(g => g.Posts)
                .ThenInclude(p => p.Comments)
                    .ThenInclude(c => c.User)
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

    [HttpPost]
    public async Task<IActionResult> CreatePost(Guid groupId, string? contentText, IFormFile? mediaFile)
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!string.IsNullOrWhiteSpace(contentText) || mediaFile != null)
        {
            await postService.CreatePostAsync(user.Id, groupId, contentText, mediaFile);
        }

        return RedirectToAction("Details", new { id = groupId });
    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        try
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            Guid? groupId = await postService.SoftDeletePostAsync(user.Id, postId);
            if (groupId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Details), new { id = groupId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
