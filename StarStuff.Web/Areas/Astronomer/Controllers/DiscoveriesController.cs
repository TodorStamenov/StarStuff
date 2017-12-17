namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.Discoveries;
    using Services;
    using Services.Astronomer;
    using Services.Astronomer.Models.Astronomers;
    using Services.Astronomer.Models.Discoveries;
    using Services.Moderator;
    using System.Collections.Generic;
    using System.Linq;

    public class DiscoveriesController : BaseAstronomerController
    {
        private const int DiscoveriesPerPage = 20;
        private const string Discovery = "Discovery";
        private const string StarSystem = "Star System";

        private readonly IDiscoveryService discoveryService;
        private readonly IPlanetService planetService;
        private readonly IStarService starService;
        private readonly ITelescopeService telescopeService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public DiscoveriesController(
            IDiscoveryService discoveryService,
            IPlanetService planetService,
            IStarService starService,
            ITelescopeService telescopeService,
            IUserService userService,
            UserManager<User> userManager)
        {
            this.discoveryService = discoveryService;
            this.planetService = planetService;
            this.starService = starService;
            this.telescopeService = telescopeService;
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            DiscoveryDetailsViewModel model = new DiscoveryDetailsViewModel
            {
                IsPioneer = this.discoveryService.IsPioneer(id, userId),
                IsObserver = this.discoveryService.IsObserver(id, userId),
                Discovery = this.discoveryService.Details(id),
                Stars = this.starService.Stars(id),
                Planets = this.planetService.Planets(id),
                Pioneers = this.discoveryService.Pioneers(id),
                Observers = this.discoveryService.Observers(id)
            };

            if (model.Discovery == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Confirm(int id)
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            bool success = this.discoveryService
                .Confirm(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Create()
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            DiscoveryFormViewModel model = new DiscoveryFormViewModel
            {
                Telescopes = this.GetTelescopes(),
                Astronomers = this.GetAstronomers(userId)
            };

            return View(model);
        }

        [HttpPost]
        [Log(LogType.Create, Discoveries)]
        public IActionResult Create(DiscoveryFormViewModel model)
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            if (!ModelState.IsValid)
            {
                model.Telescopes = this.GetTelescopes();
                model.Astronomers = this.GetAstronomers(userId);
                return View(model);
            }

            if (this.discoveryService.Exists(model.Discovery.StarSystem))
            {
                model.Telescopes = this.GetTelescopes();
                model.Astronomers = this.GetAstronomers(userId);

                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, StarSystem));

                return View(model);
            }

            int id = this.discoveryService.Create(
                model.Discovery.StarSystem,
                model.TelescopeId,
                userId,
                model.AstronomerIds);

            if (id <= 0)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Discovery, WebConstants.Added));

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Edit(int id)
        {
            DiscoveryFormServiceModel model = this.discoveryService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        [Log(LogType.Edit, Discoveries)]
        public IActionResult Edit(int id, DiscoveryFormServiceModel model)
        {
            string oldName = this.discoveryService.GetName(id);

            if (oldName == null)
            {
                return BadRequest();
            }

            string newName = model.StarSystem;

            if (this.discoveryService.Exists(newName)
                && oldName != newName)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, StarSystem));
                return View(model);
            }

            bool success = this.discoveryService.Edit(id, model.StarSystem);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Discovery, WebConstants.Edited));

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [Log(LogType.Delete, Discoveries)]
        public IActionResult DeletePost(int id)
        {
            bool success = this.discoveryService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Discovery, WebConstants.Deleted));

            return RedirectToAction(nameof(All));
        }

        public IActionResult Mine(int page, bool? confirmed, string astronomer)
        {
            if (page <= 0)
            {
                page = 1;
            }

            AstronomerType astronomerType = AstronomerType.All;

            if (astronomer?.ToLower() == "pioneer")
            {
                astronomerType = AstronomerType.Pioneer;
            }
            else if (astronomer?.ToLower() == "observer")
            {
                astronomerType = AstronomerType.Observer;
            }

            int userId = int.Parse(this.userManager.GetUserId(User));
            int totalDiscoveries = this.discoveryService.Total(confirmed, astronomerType, userId);

            ListMyDiscoveriesViewModel model = new ListMyDiscoveriesViewModel
            {
                Confirmed = confirmed,
                CurrentPage = page,
                AstronomerType = astronomerType.ToString(),
                TotalPages = ControllerHelpers.GetTotalPages(totalDiscoveries, DiscoveriesPerPage),
                Discoveries = this.discoveryService.All(page, DiscoveriesPerPage, userId, astronomerType, confirmed)
            };

            return View(model);
        }

        public IActionResult All(int page, bool? confirmed)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalDiscoveries = this.discoveryService.Total(confirmed);

            ListDiscoveriesViewModel model = new ListDiscoveriesViewModel
            {
                Confirmed = confirmed,
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalDiscoveries, DiscoveriesPerPage),
                Discoveries = this.discoveryService.All(page, DiscoveriesPerPage, confirmed)
            };

            return View(model);
        }

        private IEnumerable<SelectListItem> GetAstronomers(int astronomerId)
        {
            return this.userService
                .Astronomers(astronomerId)
                .Select(a => new SelectListItem
                {
                    Text = $"{a.FirstName} {a.LastName}",
                    Value = a.Id.ToString()
                });
        }

        private IEnumerable<SelectListItem> GetTelescopes()
        {
            return this.telescopeService
                .TelescopeDropdown()
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id.ToString()
                });
        }
    }
}