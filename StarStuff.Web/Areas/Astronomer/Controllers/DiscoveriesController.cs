namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Areas.Astronomer.Models.Discoveries;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using StarStuff.Data.Models;
    using StarStuff.Services.Astronomer;
    using StarStuff.Services.Astronomer.Models.Discoveries;
    using StarStuff.Services.Moderator;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DiscoveriesController : BaseAstronomerController
    {
        private const int DiscoveriesPerPage = 20;

        private readonly IDiscoveryService discoveryService;
        private readonly IPlanetService planetService;
        private readonly IStarService starService;
        private readonly ITelescopeService telescopeService;
        private readonly UserManager<User> userManager;

        public DiscoveriesController(
            IDiscoveryService discoveryService,
            IPlanetService planetService,
            IStarService starService,
            ITelescopeService telescopeService,
            UserManager<User> userManager)
        {
            this.discoveryService = discoveryService;
            this.planetService = planetService;
            this.starService = starService;
            this.telescopeService = telescopeService;
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
            DiscoveryFormViewModel model = new DiscoveryFormViewModel
            {
                Telescopes = this.GetTelescopes()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(DiscoveryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Telescopes = this.GetTelescopes();
                return View(model);
            }

            int userId = int.Parse(this.userManager.GetUserId(User));

            int id = this.discoveryService
                .Create(model.Discovery.StarSystem, model.TelescopeId, userId);

            if (id <= 0)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Discovery Successfully Added");

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
        public IActionResult Edit(int id, DiscoveryFormServiceModel model)
        {
            bool success = this.discoveryService.Edit(id, model.StarSystem);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Discovery Successfully Edited");

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public IActionResult DeletePost(int id)
        {
            bool success = this.discoveryService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Discovery Successfully Deleted");

            return RedirectToAction(nameof(All));
        }

        public IActionResult All(int page, bool? confirmed = null)
        {
            if (page <= 0)
            {
                page = 1;
            }

            ListDiscoveriesViewModel model = new ListDiscoveriesViewModel
            {
                Confirmed = confirmed,
                CurrentPage = page,
                TotalPages = this.GetTotalPages(confirmed),
                Discoveries = this.discoveryService.All(page, DiscoveriesPerPage, confirmed)
            };

            return View(model);
        }

        private int GetTotalPages(bool? confirmed = null)
        {
            int total = this.discoveryService.Total(confirmed);

            return (int)Math.Ceiling(total / (double)DiscoveriesPerPage);
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