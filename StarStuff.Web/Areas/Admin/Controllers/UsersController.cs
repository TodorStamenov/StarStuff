namespace StarStuff.Web.Areas.Admin.Controllers
{
    using Areas.Admin.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StarStuff.Services.Admin;
    using System;

    [Area("Admin")]
    [Authorize(Roles = WebConstants.AdminRole)]
    public class UsersController : Controller
    {
        private const int UsersPerPage = 10;

        private readonly IAdminUserService userService;

        public UsersController(IAdminUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult EditRoles(int id)
        {
            UserRoleEditViewModel model = new UserRoleEditViewModel
            {
                User = this.userService.Roles(id),
                Roles = this.userService.AllRoles()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddRole(int userId, string roleName)
        {
            bool success = this.userService.AddToRole(userId, roleName);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage($"User successfully added to role {roleName}");

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        public IActionResult RemoveRole(int userId, string roleName)
        {
            bool success = this.userService.RemoveFromRole(userId, roleName);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage($"User successfully removed from role {roleName}");

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        public IActionResult All(string role, int page, string search)
        {
            if (page < 1)
            {
                page = 1;
            }

            int pages = (int)Math.Ceiling(this.userService.Total(role, search) / (double)UsersPerPage);

            ListUsersViewModel model = new ListUsersViewModel
            {
                Search = search,
                CurrentPage = page,
                UserRole = role,
                TotalPages = pages,
                Users = this.userService.All(page, role, search, UsersPerPage),
                Roles = this.userService.AllRoles(),
            };

            return View(model);
        }
    }
}