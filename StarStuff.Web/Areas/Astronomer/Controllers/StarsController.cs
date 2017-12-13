namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.Astronomer;
    using Services.Astronomer.Models.Stars;

    public class StarsController : BaseAstronomerController
    {
        private const string Discoveries = "Discoveries";
        private const string Star = "Star";
        private const string Stars = "Stars";

        private readonly IStarService starService;
        private readonly IDiscoveryService discoveryService;

        public StarsController(IStarService starService, IDiscoveryService discoveryService)
        {
            this.starService = starService;
            this.discoveryService = discoveryService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        [Log(LogType.Create, Stars)]
        public IActionResult Create(int id, StarFormServiceModel model)
        {
            if (this.starService.Exists(model.Name))
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Star));
                return View(model);
            }

            if (this.discoveryService.TotalStars(id) >= WebConstants.MaxStarsPerDiscovery)
            {
                TempData.AddErrorMessage($"Maximum allowed Stars count per Discovery is {WebConstants.MaxStarsPerDiscovery}");
                return View(model);
            }

            bool success = this.starService.Create(id, model.Name, model.Temperature);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Star Successfully Created");

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
        [Log(LogType.Edit, Stars)]
        public IActionResult Edit(int id, int discoveryId, StarFormServiceModel model)
        {
            string oldName = model.Name;

            if (oldName == null)
            {
                return BadRequest();
            }

            string newName = this.starService.GetName(id);

            if (this.starService.Exists(newName)
                && oldName != newName)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.EntryExists, Star));
                return View(model);
            }

            bool success = this.starService.Edit(id, model.Name, model.Temperature);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Star Successfully Edited");

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId });
        }

        public IActionResult Delete(int id, int discoveryId)
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [Log(LogType.Delete, Stars)]
        public IActionResult DeletePost(int id, int discoveryId)
        {
            bool success = this.starService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Star Successfully Deleted");

            return RedirectToAction(nameof(DiscoveriesController.Details), Discoveries, new { id = discoveryId });
        }
    }
}