namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Astronomer;
    using Services.Astronomer.Models.Stars;

    public class StarsController : BaseAstronomerController
    {
        private const string Discoveries = "Discoveries";

        private readonly IStarService starService;

        public StarsController(IStarService starService)
        {
            this.starService = starService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Create(int id, StarFormServiceModel model)
        {
            bool success = this.starService.Create(id, model.Name, model.Temperature);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id });
        }

        public IActionResult Edit(int id, int discoveryId)
        {
            StarFormServiceModel model = this.starService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Edit(int id, int discoveryId, StarFormServiceModel model)
        {
            bool success = this.starService.Edit(id, model.Name, model.Temperature);

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
            bool success = this.starService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId });
        }
    }
}