namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using StarStuff.Services.Astronomer;

    public class StarsController : BaseAstronomerController
    {
        private readonly IStarService starService;

        public StarsController(IStarService starService)
        {
            this.starService = starService;
        }
    }
}