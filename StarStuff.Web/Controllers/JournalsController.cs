﻿namespace StarStuff.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Journals;
    using Services.Areas.Moderator;
    using Services.Areas.Moderator.Models.Journals;

    public class JournalsController : Controller
    {
        private const int JournalsPerPage = 10;

        private readonly IJournalService journalService;

        public JournalsController(IJournalService journalService)
        {
            this.journalService = journalService;
        }

        public IActionResult Details(int id)
        {
            JournalDetailsServiceModel model = this.journalService.Details(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        public IActionResult All(int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalJournals = this.journalService.Total();

            ListJournalsViewModel model = new ListJournalsViewModel
            {
                CurrentPage = page,
                TotalEntries = totalJournals,
                EntriesPerPage = JournalsPerPage,
                Journals = this.journalService.All(page, JournalsPerPage)
            };

            return View(model);
        }
    }
}