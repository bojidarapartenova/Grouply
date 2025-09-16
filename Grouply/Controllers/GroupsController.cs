using Microsoft.AspNetCore.Mvc;
using Grouply.Models;
using Grouply.Data;
using Microsoft.EntityFrameworkCore;

public class GroupsController : Controller
{
    private readonly GrouplyDbContext _context;

    public GroupsController(GrouplyDbContext context)
    {
        _context = context;
    }

    // In GroupsController
    public IActionResult Index()
    {
        var groups = _context.Groups.Where(g => !g.IsDeleted).ToList();
        return View(groups);
    }

    public IActionResult Details(Guid id)
    {
        var group = _context.Groups
            .Include(g => g.CreatedBy)
            .Include(g => g.GroupMembers)
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == id && !g.IsDeleted);

        if (group == null) return NotFound();

        return View(group);
    }
}
