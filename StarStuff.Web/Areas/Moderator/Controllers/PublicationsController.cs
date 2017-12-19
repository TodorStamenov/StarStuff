namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.Publications;
    using Services.Astronomer;
    using Services.Moderator;
    using Services.Moderator.Models.Publications;
    using System.Linq;

    public class PublicationsController : BaseModeratorController
    {
        private const string Publication = "Publication";
        private const string Publications = "Publications";

        private readonly IPublicationService publicationService;
        private readonly IDiscoveryService discoveryService;
        private readonly IJournalService journalService;
        private readonly UserManager<User> userManager;

        public PublicationsController(
            IPublicationService publicationService,
            IDiscoveryService discoveryService,
            IJournalService journalService,
            UserManager<User> userManager)
        {
            this.publicationService = publicationService;
            this.discoveryService = discoveryService;
            this.journalService = journalService;
            this.userManager = userManager;
        }

        public IActionResult Create(int id)
        {
            PublicationFormViewModel model = new PublicationFormViewModel();
            this.PopulateCreateViewModel(id, model);

            return View(model);
        }

        [HttpPost]
        [Log(LogType.Create, Publications)]
        public IActionResult Create(int id, PublicationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.PopulateCreateViewModel(id, model);
                return View(model);
            }

            if (this.publicationService.TitleExists(model.Publication.Title))
            {
                this.PopulateCreateViewModel(id, model);
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Publication));
                return View(model);
            }

            if (this.publicationService.Exists(id, model.DiscoveryId))
            {
                this.PopulateCreateViewModel(id, model);
                TempData.AddErrorMessage("Publication from this Journal for this Discovery already exists!");
                return View(model);
            }

            int authorId = int.Parse(this.userManager.GetUserId(User));

            int publicationId = this.publicationService.Create(
                model.Publication.Title,
                model.Publication.Content,
                model.DiscoveryId,
                id,
                authorId);

            if (publicationId <= 0)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Publication, WebConstants.Added));

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
        [Log(LogType.Edit, Publications)]
        public IActionResult Edit(int id, PublicationFormServiceModel model)
        {
            string oldTitle = this.publicationService.GetTitle(id);

            if (oldTitle == null)
            {
                return BadRequest();
            }

            string newTitle = model.Title;

            if (this.publicationService.TitleExists(newTitle)
                && oldTitle != newTitle)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Publication));
                return View(model);
            }

            bool success = this.publicationService.Edit(
                id,
                model.Title,
                model.Content);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Publication, WebConstants.Edited));

            return RedirectToAction(Details, Publications, new { id, area = string.Empty });
        }

        private void PopulateCreateViewModel(int journalId, PublicationFormViewModel model)
        {
            model.JournalName = this.journalService.GetName(journalId);
            model.Discoveries = this.discoveryService
                .DiscoveryDropdown(journalId)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.StarSystem
                });
        }
    }
}