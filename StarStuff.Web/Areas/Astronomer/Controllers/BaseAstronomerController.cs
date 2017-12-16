namespace StarStuff.Web.Areas.Astronomer.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(WebConstants.AstronomerArea)]
    [Authorize(Roles = WebConstants.AstronomerRole)]
    public class BaseAstronomerController : Controller
    {
        protected const string Discoveries = "Discoveries";
    }
}