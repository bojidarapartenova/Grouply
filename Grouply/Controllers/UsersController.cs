using Microsoft.AspNetCore.Mvc;
using Grouply.Models;
using Grouply.Data;
using Microsoft.EntityFrameworkCore;
using Grouply.Controllers;
using Microsoft.AspNetCore.Identity;

public class UsersController : BaseController
{
    private readonly UserManager<ApplicationUser> userManager;
    public UsersController(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<IActionResult> Profile(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
}
