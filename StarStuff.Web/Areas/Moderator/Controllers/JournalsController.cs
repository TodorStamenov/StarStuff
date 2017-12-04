﻿namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using StarStuff.Services.Moderator;
    using StarStuff.Services.Moderator.Models.Journals;

    public class JournalsController : BaseModeratorController
    {
        private const string JournalDetailsUrl = "/Journals/Details/{0}";

        private readonly IJournalService journalService;

        public JournalsController(IJournalService journalService)
        {
            this.journalService = journalService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Create(JournalFormServiceModel model)
        {
            int id = this.journalService.Create(model.Name, model.Description, model.ImageUrl);

            TempData.AddSuccessMessage("Journal Successfully Added");

            return Redirect(string.Format(JournalDetailsUrl, id));
        }

        public IActionResult Edit(int id)
        {
            JournalFormServiceModel model = this.journalService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Edit(int id, JournalFormServiceModel model)
        {
            bool success = this.journalService.Edit(id, model.Name, model.Description, model.ImageUrl);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Journal Successfully Edited");

            return Redirect(string.Format(JournalDetailsUrl, id));
        }
    }
}