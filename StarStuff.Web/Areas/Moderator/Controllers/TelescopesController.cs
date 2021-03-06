﻿namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Areas.Moderator;
    using Services.Areas.Moderator.Models.Telescopes;

    public class TelescopesController : BaseModeratorController
    {
        private const string Telescope = "Telescope";
        private const string Telescopes = "Telescopes";

        private readonly ITelescopeService telescopeService;

        public TelescopesController(ITelescopeService telescopeService)
        {
            this.telescopeService = telescopeService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        [Log(nameof(Create), Telescopes)]
        public IActionResult Create(TelescopeFormServiceModel model)
        {
            if (this.telescopeService.Exists(model.Name))
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Telescope));
                return View(model);
            }

            int id = this.telescopeService.Create(
                model.Name,
                model.Location,
                model.Description,
                model.MirrorDiameter,
                model.ImageUrl);

            if (id <= 0)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Telescope, WebConstants.Added));

            return RedirectToAction(Details, Telescopes, new { area = string.Empty, id });
        }

        public IActionResult Edit(int id)
        {
            TelescopeFormServiceModel model = this.telescopeService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        [Log(nameof(Edit), Telescopes)]
        public IActionResult Edit(int id, TelescopeFormServiceModel model)
        {
            string oldName = this.telescopeService.GetName(id);

            if (oldName == null)
            {
                return BadRequest();
            }

            string newName = model.Name;

            if (this.telescopeService.Exists(newName)
                && oldName != newName)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Telescope));
                return View(model);
            }

            bool success = this.telescopeService.Edit(
                id,
                model.Name,
                model.Location,
                model.Description,
                model.MirrorDiameter,
                model.ImageUrl);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Telescope, WebConstants.Edited));

            return RedirectToAction(Details, Telescopes, new { area = string.Empty, id });
        }
    }
}