namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using StarStuff.Services.Astronomer;

    public class PlanetsController : BaseAstronomerController
    {
        private readonly IPlanetService planetService;

        public PlanetsController(IPlanetService planetService)
        {
            this.planetService = planetService;
        }
    }
}