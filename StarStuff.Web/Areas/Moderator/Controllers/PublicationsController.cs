﻿namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Areas.Moderator.Models.Publications;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using StarStuff.Services.Astronomer;
    using StarStuff.Services.Moderator;
    using StarStuff.Services.Moderator.Models.Publications;
    using System.Collections.Generic;
    using System.Linq;

    public class PublicationsController : BaseModeratorController
    {
        private const string Publications = "Publications";
        private const string Details = "Details";

        private readonly IPublicationService publicationService;
        private readonly IDiscoveryService discoveryService;
        private readonly IJournalService journalService;

        public PublicationsController(IPublicationService publicationService, IDiscoveryService discoveryService, IJournalService journalService)
        {
            this.publicationService = publicationService;
            this.discoveryService = discoveryService;
            this.journalService = journalService;
        }

        public IActionResult Create(int id)
        {
            CreatePublicationFormViewModel model = new CreatePublicationFormViewModel
            {
                Discoveries = this.GetDiscoveries(id)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(int id, CreatePublicationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Discoveries = this.GetDiscoveries(id);
                return View(model);
            }

            int publicationId = this.publicationService.Create(
                model.Publication.Content,
                model.DiscoveryId,
                id);

            if (publicationId <= 0)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Publication Successfully Added");

            return RedirectToAction(Details, Publications, new { id = publicationId, area = string.Empty });
        }

        public IActionResult Edit(int id)
        {
            PublicationFormServiceModel model = this.publicationService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Edit(int id, PublicationFormServiceModel model)
        {
            bool success = this.publicationService.Edit(
                id,
                model.Content);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Publication Successfully Edited");

            return RedirectToAction(Details, Publications, new { id, area = string.Empty });
        }

        private IEnumerable<SelectListItem> GetDiscoveries(int journalId)
        {
            return this.discoveryService
                .DiscoveryDropdown(journalId)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.StarSystem
                });
        }
    }
}