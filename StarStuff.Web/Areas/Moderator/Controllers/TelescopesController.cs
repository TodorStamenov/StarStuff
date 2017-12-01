namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StarStuff.Services.Moderator;

    public class TelescopesController : BaseModeratorController
    {
        private readonly ITelescopeService telescopeService;

        public TelescopesController(ITelescopeService telescopeService)
        {
            this.telescopeService = telescopeService;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            return View();
        }
    }
}