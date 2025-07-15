using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models.DBmodels;

namespace ticketApp.ViewComponents;

public class UserInfoViewComponent : ViewComponent
{
    private readonly UserManager<Person> _userManager;
    private readonly DBContext db;

    public UserInfoViewComponent(UserManager<Person> userManager, DBContext db)
    {
        _userManager = userManager;
        this.db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!User.Identity.IsAuthenticated)
            return View(null); // Will show guest message

        var currentUser = await _userManager.GetUserAsync(UserClaimsPrincipal);
        Employee emp = db.Employees.Include(e => e.Person).FirstOrDefault(e => e.personId == currentUser.Id);

        return View(emp);
    }
}
