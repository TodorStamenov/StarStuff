namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StarStuff.Data.Models;
    using StarStuff.Services.Models.Users;
    using StarStuff.Services.Moderator;
    using System.Collections.Generic;

    public class AstronomersController : BaseModeratorController
    {
        private readonly IModeratorUserService userService;
        private readonly UserManager<User> userManager;

        public AstronomersController(IModeratorUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult ApplicationDetails(int id)
        {
            UserDetailsServiceModel model =
                this.userService.ApplicationDetails(id);

            return View(model);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            this.userService.Approve(id);

            return RedirectToAction(nameof(Applications));
        }

        [HttpPost]
        public IActionResult Deny(int id)
        {
            this.userService.Deny(id);

            return RedirectToAction(nameof(Applications));
        }

        public IActionResult Applications()
        {
            IEnumerable<ListUsersServiceModel> model =
                this.userService.Applications();

            return View(model);
        }
    }
}