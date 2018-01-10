namespace StarStuff.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Astronomers;
    using Services;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private const int AstronomersPerPage = 6;

        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Astronomers(int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalAstronomers = this.userService.TotalAstronomers();

            ListAstronomersViewModel model = new ListAstronomersViewModel
            {
                CurrentPage = page,
                TotalEntries = totalAstronomers,
                EntriesPerPage = AstronomersPerPage,
                Astronomers = this.userService.Astronomers(page, AstronomersPerPage)
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}