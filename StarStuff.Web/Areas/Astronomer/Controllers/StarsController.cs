﻿namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Data;
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Areas.Astronomer;
    using Services.Areas.Astronomer.Models.Stars;

    public class StarsController : BaseAstronomerController
    {
        private const string Star = "Star";
        private const string Stars = "Stars";

        private readonly IStarService starService;
        private readonly IDiscoveryService discoveryService;

        public StarsController(IStarService starService, IDiscoveryService discoveryService)
        {
            this.starService = starService;
            this.discoveryService = discoveryService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        [Log(nameof(Create), Stars)]
        public IActionResult Create(int id, StarFormServiceModel model)
        {
            if (this.starService.Exists(model.Name))
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Star));
                return View(model);
            }

            if (this.discoveryService.TotalStars(id) >= DataConstants.DiscoveryConstants.MaxStarsPerDiscovery)
            {
                TempData.AddErrorMessage(string.Format(
                    DataConstants.DiscoveryConstants.MaxStarsPerDiscoveryErrorMessage,
                    DataConstants.DiscoveryConstants.MaxStarsPerDiscovery));

                return View(model);
            }

            bool success = this.starService.Create(id, model.Name, model.Temperature);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Star, WebConstants.Added));

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id, area = WebConstants.AstronomerArea });
        }

        public IActionResult Edit(int id, int discoveryId)
        {
            StarFormServiceModel model = this.starService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        [Log(nameof(Edit), Stars)]
        public IActionResult Edit(int id, int discoveryId, StarFormServiceModel model)
        {
            string oldName = this.starService.GetName(id);

            if (oldName == null)
            {
                return BadRequest();
            }

            string newName = model.Name;

            if (this.starService.Exists(newName)
                && oldName != newName)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Star));
                return View(model);
            }

            bool success = this.starService.Edit(id, model.Name, model.Temperature);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Star, WebConstants.Edited));

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId, area = WebConstants.AstronomerArea });
        }

        public IActionResult Delete(int id, int discoveryId)
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [Log(nameof(Delete), Stars)]
        public IActionResult DeletePost(int id, int discoveryId)
        {
            bool success = this.starService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Star, WebConstants.Deleted));

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId, area = WebConstants.AstronomerArea });
        }
    }
}