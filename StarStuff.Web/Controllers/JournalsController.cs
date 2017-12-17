namespace StarStuff.Web.Controllers
{
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Models.Journals;
    using Services.Moderator;
    using Services.Moderator.Models.Journals;

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
                TotalPages = ControllerHelpers.GetTotalPages(totalJournals, JournalsPerPage),
                Journals = this.journalService.All(page, JournalsPerPage)
            };

            return View(model);
        }
    }
}