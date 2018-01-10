namespace StarStuff.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Logs;
    using Models.Users;
    using Services.Areas.Admin;
    using System;
    using System.Threading.Tasks;

    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdminRole)]
    public class UsersController : Controller
    {
        private const int UsersPerPage = 10;
        private const int LogsPerPage = 10;
        private const string UserRole = "UserRole";
        private const string UsersTable = "Users";

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

            if (user == null)
            {
                return BadRequest();
            }

            UserRoleEditViewModel model = new UserRoleEditViewModel
            {
                IsUserLocked = await userManager.IsLockedOutAsync(user),
                User = this.userService.Roles(id),
                Roles = this.userService.AllRoles()
            };

            return View(model);
        }

        [HttpPost]
        [Log(nameof(AddRole), UserRole)]
        public IActionResult AddRole(int userId, string roleName)
        {
            string username = this.userService.GetUsername(userId);

            if (username == null)
            {
                return BadRequest();
            }

            bool success = this.userService.AddToRole(userId, roleName);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.UserAddedToRole, username, roleName));

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        [Log(nameof(RemoveRole), UserRole)]
        public IActionResult RemoveRole(int userId, string roleName)
        {
            string username = this.userService.GetUsername(userId);

            if (username == null)
            {
                return BadRequest();
            }

            bool success = this.userService.RemoveFromRole(userId, roleName);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.UserRemovedFormRole, username, roleName));

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        [Log(nameof(Lock), UsersTable)]
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

            TempData.AddSuccessMessage(string.Format(WebConstants.UserLocked, username));

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        [HttpPost]
        [Log(nameof(Unlock), UsersTable)]
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

            TempData.AddSuccessMessage(string.Format(WebConstants.UserUnlocked, username));

            return RedirectToAction(nameof(EditRoles), new { id = userId });
        }

        public IActionResult Logs(int page, string search)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalLogs = this.userService.TotalLogs(search);

            ListLogsViewModel model = new ListLogsViewModel
            {
                Search = search,
                CurrentPage = page,
                TotalEntries = totalLogs,
                EntriesPerPage = LogsPerPage,
                Logs = this.userService.Logs(page, LogsPerPage, search),
            };

            return View(model);
        }

        public IActionResult Users(int page, bool locked, string userRole, string search)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalUsers = this.userService.TotalUsers(userRole, search, locked);

            ListUsersViewModel model = new ListUsersViewModel
            {
                Search = search,
                CurrentPage = page,
                UserRole = userRole,
                TotalEntries = totalUsers,
                Locked = locked,
                EntriesPerPage = UsersPerPage,
                Users = this.userService.All(page, UsersPerPage, userRole, search, locked),
                Roles = this.userService.AllRoles(),
            };

            return View(model);
        }
    }
}