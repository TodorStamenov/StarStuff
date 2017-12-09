namespace StarStuff.Web.Areas.Admin.Controllers
{
    using Models.Logs;
    using Models.Users;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Admin;
    using System;

    [Area("Admin")]
    [Authorize(Roles = WebConstants.AdminRole)]
    public class UsersController : Controller
    {
        private const int UsersPerPage = 10;
        private const int LogsPerPage = 10;

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

        public IActionResult Logs(int page, string search)
        {
            if (page < 1)
            {
                page = 1;
            }

            search = search ?? string.Empty;

            int totalLogs = this.userService.Total(search);

            ListLogsViewModel model = new ListLogsViewModel
            {
                Search = search,
                CurrentPage = page,
                TotalPages = this.GetToltalPages(totalLogs),
                Logs = this.userService.Logs(page, LogsPerPage, search),
            };

            return View(model);
        }

        public IActionResult All(int page, string role, string search)
        {
            if (page < 1)
            {
                page = 1;
            }

            int totalUsers = this.userService.Total(role, search);

            ListUsersViewModel model = new ListUsersViewModel
            {
                Search = search,
                CurrentPage = page,
                UserRole = role,
                TotalPages = this.GetToltalPages(totalUsers),
                Users = this.userService.All(page, role, search, UsersPerPage),
                Roles = this.userService.AllRoles(),
            };

            return View(model);
        }

        private int GetToltalPages(int totalEntries)
        {
            return (int)Math.Ceiling(totalEntries / (double)UsersPerPage);
        }
    }
}