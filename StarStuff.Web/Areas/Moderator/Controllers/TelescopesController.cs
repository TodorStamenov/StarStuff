namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using StarStuff.Services.Moderator;
    using StarStuff.Services.Moderator.Models.Telescopes;

    public class TelescopesController : BaseModeratorController
    {
        private const string TelescopeDetailsUrl = "/Telescopes/Details/{0}";

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
        public IActionResult Create(TelescopeFormServiceModel model)
        {
            int id = this.telescopeService.Create(
                model.Name,
                model.Location,
                model.Description,
                model.MirrorDiameter,
                model.ImageUrl);

            TempData.AddSuccessMessage("Telescope Successfully Added");

            return Redirect(string.Format(TelescopeDetailsUrl, id));
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
        public IActionResult Edit(int id, TelescopeFormServiceModel model)
        {
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

            TempData.AddSuccessMessage("Telescope Successfully Edited");

            return Redirect(string.Format(TelescopeDetailsUrl, id));
        }
    }
}