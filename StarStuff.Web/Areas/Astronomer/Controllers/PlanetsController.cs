namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Astronomer;
    using Services.Astronomer.Models.Planets;

    public class PlanetsController : BaseAstronomerController
    {
        private const string Discoveries = "Discoveries";

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
        public IActionResult Create(int id, PlanetFormServiceModel model)
        {
            bool success = this.planetService.Create(id, model.Name, model.Mass);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id });
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
        public IActionResult Edit(int id, int discoveryId, PlanetFormServiceModel model)
        {
            bool success = this.planetService.Edit(id, model.Name, model.Mass);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId });
        }

        public IActionResult Delete(int id, int discoveryId)
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public IActionResult DeletePost(int id, int discoveryId)
        {
            bool success = this.planetService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId });
        }
    }
}