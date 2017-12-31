namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Data.Models;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Models.Users;
    using Services.Areas.Moderator;
    using System.Collections.Generic;

    public class AstronomersController : BaseModeratorController
    {
        private const string Users = "Users";

        private readonly IModeratorUserService userService;

        public AstronomersController(IModeratorUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult ApplicationDetails(int id)
        {
            UserDetailsServiceModel model =
                this.userService.ApplicationDetails(id);

            return View(model);
        }

        [HttpPost]
        [Log(nameof(Approve), Users)]
        public IActionResult Approve(int id)
        {
            string username = this.userService.GetUsername(id);

            if (username == null)
            {
                return BadRequest();
            }

            this.userService.Approve(id);

            TempData.AddSuccessMessage($"Astronomer application for user {username} was successfully approved");

            return RedirectToAction(nameof(Applications));
        }

        [HttpPost]
        [Log(nameof(Deny), Users)]
        public IActionResult Deny(int id)
        {
            string username = this.userService.GetUsername(id);

            if (username == null)
            {
                return BadRequest();
            }

            this.userService.Deny(id);

            TempData.AddSuccessMessage($"Astronomer application for user {username} was successfully denied");

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