namespace StarStuff.Web.Areas.Moderator.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(WebConstants.ModeratorArea)]
    [Authorize(Roles = WebConstants.ModeratorRole)]
    public class BaseModeratorController : Controller
    {
        protected const string Details = "Details";
    }
}