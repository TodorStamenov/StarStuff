namespace StarStuff.Web.Controllers
{
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Models.Telescopes;
    using Services.Moderator;
    using Services.Moderator.Models.Telescopes;

    public class TelescopesController : Controller
    {
        private const int TelescopesPerPage = 10;

        private readonly ITelescopeService telescopeService;

        public TelescopesController(ITelescopeService telescopeService)
        {
            this.telescopeService = telescopeService;
        }

        public IActionResult Details(int id)
        {
            TelescopeDetailsServiceModel model = this.telescopeService.Details(id);

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

            int totalTelescopes = this.telescopeService.Total();

            ListTelescopesViewModel model = new ListTelescopesViewModel
            {
                CurrentPage = page,
                TotalPages = ControllerHelpers.GetTotalPages(totalTelescopes, TelescopesPerPage),
                Telescopes = this.telescopeService.All(page, TelescopesPerPage)
            };

            return View(model);
        }
    }
}