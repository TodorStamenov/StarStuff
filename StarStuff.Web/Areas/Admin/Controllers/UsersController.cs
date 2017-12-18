namespace StarStuff.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Logs;
    using Models.Users;
    using Services.Admin;
    using System;
    using System.Threading.Tasks;

    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdminRole)]
    public class UsersController : Controller
    {
        private const int UsersPerPage = 10;
        private const int LogsPerPage = 10;

        private readonly IAdminUserService userService;
        private readonly UserManager<User> userManager;

        public UsersController(IAdminUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> EditRoles(int id)
        {
            User user = await userManager.FindByIdAsync(id.ToString());

            UserRoleEditViewModel model = new UserRoleEditViewModel
            {
                IsUserLocked = await userManager.IsLockedOutAsync(user),
                User = this.userService.Roles(id),
                Roles = this.userService.AllRoles()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(int userId, string roleName)
        {
            bool success = this.userService.AddToRole(userId, roleName);

            if (!success)
            {
                return BadRequest();
            }

            User user = await this.userManager.FindByIdAsync(userId.ToString());

            TempData.AddSuccessMessage($"User {user.UserName} successfully added to role {roleName}");

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(int userId, string roleName)
        {
            bool success = this.userService.RemoveFromRole(userId, roleName);

            if (!success)
            {
                return BadRequest();
            }

            User user = await this.userManager.FindByIdAsync(userId.ToString());

            TempData.AddSuccessMessage($"User {user.UserName} successfully removed from role {roleName}");

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> Lock(int userId)
        {
            User user = await this.userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return BadRequest();
            }

            if (await this.userManager.IsLockedOutAsync(user))
            {
                return BadRequest();
            }

            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddYears(10));

            string username = await this.userManager.GetUserNameAsync(user);

            TempData.AddSuccessMessage($"User {username} successfully Locked");

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> Unlock(int userId)
        {
            User user = await this.userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return BadRequest();
            }

            if (!await this.userManager.IsLockedOutAsync(user))
            {
                return BadRequest();
            }

            await userManager.SetLockoutEnabledAsync(user, false);

            string username = await this.userManager.GetUserNameAsync(user);

            TempData.AddSuccessMessage($"User {username} successfully Unlocked");

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        public IActionResult Logs(int page, string search)
        {
            if (page <= 0)
            {
                page = 1;
            }

            search = search ?? string.Empty;

            int totalLogs = this.userService.Total(search);

            ListLogsViewModel model = new ListLogsViewModel
            {
                Search = search,
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalLogs, LogsPerPage),
                Logs = this.userService.Logs(page, LogsPerPage, search),
            };

            return View(model);
        }

        public IActionResult Users(int page, string role, string search)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalUsers = this.userService.Total(role, search);

            ListUsersViewModel model = new ListUsersViewModel
            {
                Search = search,
                CurrentPage = page,
                UserRole = role,
                TotalPages = ControllerHelpers.GetTotalPages(totalUsers, UsersPerPage),
                Users = this.userService.All(page, role, search, UsersPerPage),
                Roles = this.userService.AllRoles(),
            };

            return View(model);
        }
    }
}