namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Areas.Astronomer;
    using Services.Areas.Astronomer.Models.Planets;

    public class PlanetsController : BaseAstronomerController
    {
        private const string Planet = "Planet";
        private const string Planets = "Planets";

        private readonly IPlanetService planetService;

        public PlanetsController(IPlanetService planetService)
        {
            this.planetService = planetService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        [Log(nameof(Create), Planets)]
        public IActionResult Create(int id, PlanetFormServiceModel model)
        {
            if (this.planetService.Exists(model.Name))
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Planet));
                return View(model);
            }

            bool success = this.planetService.Create(id, model.Name, model.Mass);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Planet, WebConstants.Added));

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id, area = WebConstants.AstronomerArea });
        }

        public IActionResult Edit(int id, int discoveryId)
        {
            PlanetFormServiceModel model = this.planetService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        [Log(nameof(Edit), Planets)]
        public IActionResult Edit(int id, int discoveryId, PlanetFormServiceModel model)
        {
            string oldName = this.planetService.GetName(id);

            if (oldName == null)
            {
                return BadRequest();
            }

            string newName = model.Name;

            if (this.planetService.Exists(newName)
                && oldName != newName)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Planet));
                return View(model);
            }

            bool success = this.planetService.Edit(id, model.Name, model.Mass);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Planet, WebConstants.Edited));

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId, area = WebConstants.AstronomerArea });
        }

        public IActionResult Delete(int id, int discoveryId)
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [Log(nameof(Delete), Planets)]
        public IActionResult DeletePost(int id, int discoveryId)
        {
            bool success = this.planetService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Planet, WebConstants.Deleted));

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId, area = WebConstants.AstronomerArea });
        }
    }
}