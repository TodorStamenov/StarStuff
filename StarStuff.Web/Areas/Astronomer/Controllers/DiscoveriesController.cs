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
    using Services.Areas.Astronomer;
    using Services.Areas.Astronomer.Models.Astronomers;
    using Services.Areas.Astronomer.Models.Discoveries;
    using Services.Areas.Moderator;
    using System.Collections.Generic;
    using System.Linq;

    public class DiscoveriesController : BaseAstronomerController
    {
        private const int DiscoveriesPerPage = 20;
        private const string Discovery = "Discovery";
        private const string Star = "Star";
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
            int astronomerId = int.Parse(this.userManager.GetUserId(User));

            bool success = this.discoveryService.Confirm(id, astronomerId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Create()
        {
            int astronomerId = int.Parse(this.userManager.GetUserId(User));

            DiscoveryFormViewModel model = new DiscoveryFormViewModel();
            this.PopulateCreateViewModel(astronomerId, model);

            return View(model);
        }

        [HttpPost]
        [Log(LogType.Create, Discoveries)]
        public IActionResult Create(DiscoveryFormViewModel model)
        {
            int astronomerId = int.Parse(this.userManager.GetUserId(User));

            if (!ModelState.IsValid)
            {
                this.PopulateCreateViewModel(astronomerId, model);
                return View(model);
            }

            if (this.discoveryService.Exists(model.Discovery.StarSystem))
            {
                this.PopulateCreateViewModel(astronomerId, model);
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, StarSystem));
                return View(model);
            }

            if (this.starService.Exists(model.Star.Name))
            {
                this.PopulateCreateViewModel(astronomerId, model);
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Star));
                return View(model);
            }

            int discoveryId = this.discoveryService.Create(
                model.Discovery.StarSystem,
                model.Discovery.Distance,
                model.TelescopeId,
                astronomerId,
                model.AstronomerIds);

            if (discoveryId <= 0)
            {
                return BadRequest();
            }

            bool success = this.starService.Create(discoveryId, model.Star.Name, model.Star.Temperature);

            if (!success)
            {
                this.discoveryService.Delete(discoveryId);
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Discovery, WebConstants.Added));

            return RedirectToAction(nameof(Details), new { id = discoveryId });
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

            bool success = this.discoveryService.Edit(id, model.StarSystem, model.Distance);

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

        public IActionResult Mine(int page, string search, bool? confirmed, string astronomer)
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

            int astronomerId = int.Parse(this.userManager.GetUserId(User));
            int totalDiscoveries = this.discoveryService.Total(search, confirmed, astronomerId, astronomerType);

            ListMyDiscoveriesViewModel model = new ListMyDiscoveriesViewModel
            {
                Confirmed = confirmed,
                CurrentPage = page,
                Search = search,
                Astronomer = astronomerType.ToString(),
                TotalPages = ControllerHelpers.GetTotalPages(totalDiscoveries, DiscoveriesPerPage),
                Discoveries = this.discoveryService.All(page, DiscoveriesPerPage, search, confirmed, astronomerId, astronomerType)
            };

            return View(model);
        }

        public IActionResult All(int page, string search, bool? confirmed)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalDiscoveries = this.discoveryService.Total(search, confirmed);

            ListDiscoveriesViewModel model = new ListDiscoveriesViewModel
            {
                Confirmed = confirmed,
                CurrentPage = page,
                Search = search,
                TotalPages = ControllerHelpers.GetTotalPages(totalDiscoveries, DiscoveriesPerPage),
                Discoveries = this.discoveryService.All(page, DiscoveriesPerPage, search, confirmed)
            };

            return View(model);
        }

        private void PopulateCreateViewModel(int astronomerId, DiscoveryFormViewModel model)
        {
            model.Astronomers = this.GetAstronomers(astronomerId);
            model.Telescopes = this.GetTelescopes();
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