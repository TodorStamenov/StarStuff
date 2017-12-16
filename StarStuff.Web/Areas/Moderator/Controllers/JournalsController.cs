namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Moderator;
    using Services.Moderator.Models.Journals;

    public class JournalsController : BaseModeratorController
    {
        private const string Journal = "Journal";
        private const string Journals = "Journals";

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
        [Log(LogType.Create, Journals)]
        public IActionResult Create(JournalFormServiceModel model)
        {
            if (this.journalService.Exists(model.Name))
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Journal));
                return View(model);
            }

            int id = this.journalService.Create(
                model.Name,
                model.Description,
                model.ImageUrl);

            if (id <= 0)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Journal, WebConstants.Added));

            return RedirectToAction(Details, Journals, new { area = string.Empty, id });
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
        [Log(LogType.Edit, Journals)]
        public IActionResult Edit(int id, JournalFormServiceModel model)
        {
            string oldName = this.journalService.GetName(id);

            if (oldName == null)
            {
                return BadRequest();
            }

            string newName = model.Name;

            if (this.journalService.Exists(newName)
                && oldName != newName)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Journal));
                return View(model);
            }

            bool success = this.journalService.Edit(
                id,
                model.Name,
                model.Description,
                model.ImageUrl);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Journal, WebConstants.Edited));

            return RedirectToAction(Details, Journals, new { area = string.Empty, id });
        }
    }
}