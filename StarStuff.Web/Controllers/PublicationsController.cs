namespace StarStuff.Web.Controllers
{
    using Data.Models;
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Publications;
    using Services;
    using Services.Areas.Moderator;

    public class PublicationsController : Controller
    {
        private const int PublicationsPerPage = 10;
        private const int CommentsPerPage = 10;

        private readonly ICommentService commentService;
        private readonly IJournalService journalService;
        private readonly ITelescopeService telescopeService;
        private readonly IPublicationService publicationService;
        private readonly UserManager<User> userManager;

        public PublicationsController(
            ICommentService commentService,
            IJournalService journalService,
            ITelescopeService telescopeService,
            IPublicationService publicationService,
            UserManager<User> userManager)
        {
            this.commentService = commentService;
            this.journalService = journalService;
            this.telescopeService = telescopeService;
            this.publicationService = publicationService;
            this.userManager = userManager;
        }

        public IActionResult Details(int id, int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int? userId = null;

            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(this.userManager.GetUserId(User));
            }

            int totalComments = this.commentService.Total(id);

            PublicationDetailsViewModel model = new PublicationDetailsViewModel
            {
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalComments, CommentsPerPage),
                Publication = this.publicationService.Details(id),
                Comments = this.commentService.All(id, page, CommentsPerPage, userId)
            };

            if (model.Publication == null)
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

            int totalEntries = this.publicationService.Total();

            ListPublicationsViewModel model = new ListPublicationsViewModel
            {
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalEntries, PublicationsPerPage),
                Publications = this.publicationService.All(page, PublicationsPerPage)
            };

            return View(model);
        }

        public IActionResult ByJournal(int id, int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalEntries = this.publicationService.TotalByJournal(id);

            ListPublicationsByJournalViewModel model = new ListPublicationsByJournalViewModel
            {
                JournalId = id,
                JournalName = this.journalService.GetName(id),
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalEntries, PublicationsPerPage),
                Publications = this.publicationService.AllByJournal(id, page, PublicationsPerPage)
            };

            if (model.Publications == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        public IActionResult ByTelescope(int id, int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalEntries = this.publicationService.TotalByTelescope(id);

            ListPublicationsByTelescopeViewModel model = new ListPublicationsByTelescopeViewModel
            {
                TelescopeId = id,
                TelescopeName = this.telescopeService.GetName(id),
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalEntries, PublicationsPerPage),
                Publications = this.publicationService.AllByTelescope(id, page, PublicationsPerPage)
            };

            if (model.Publications == null)
            {
                return BadRequest();
            }

            return View(model);
        }
    }
}