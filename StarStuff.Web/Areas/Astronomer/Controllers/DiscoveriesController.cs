namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using StarStuff.Services.Astronomer;

    public class DiscoveriesController : BaseAstronomerController
    {
        private readonly IDiscoveryService discoveryService;

        public DiscoveriesController(IDiscoveryService discoveryService)
        {
            this.discoveryService = discoveryService;
        }
    }
}