namespace StarStuff.Web.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Services.Models.Comments;

    [Authorize]
    public class CommentsController : Controller
    {
        private const string Comment = "Comment";
        private const string Publications = "Publications";

        private readonly ICommentService commentService;
        private readonly UserManager<User> userManager;

        public CommentsController(ICommentService commentService, UserManager<User> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        public IActionResult Create(int id, int page)
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Create(int id, int page, CommentFormServiceModel model)
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            bool success = this.commentService.Create(id, userId, model.Content);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Comment, WebConstants.Added));

            return RedirectToAction(
                nameof(PublicationsController.Details),
                Publications,
                new { area = string.Empty, id, page });
        }

        public IActionResult Edit(int id, int publicationId, int page)
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            bool canEdit = this.commentService.CanEdit(id, userId);

            if (!canEdit && !User.IsInRole(WebConstants.ModeratorRole))
            {
                return Unauthorized();
            }

            CommentFormServiceModel model = this.commentService.GetForm(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Edit(int id, int publicationId, int page, CommentFormServiceModel model)
        {
            int userId = int.Parse(this.userManager.GetUserId(User));

            bool canEdit = this.commentService.CanEdit(id, userId);

            if (!canEdit && !User.IsInRole(WebConstants.ModeratorRole))
            {
                return Unauthorized();
            }

            bool success = this.commentService.Edit(id, model.Content);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Comment, WebConstants.Edited));

            return RedirectToAction(
                nameof(PublicationsController.Details),
                Publications,
                new { area = string.Empty, id = publicationId, page });
        }

        [Authorize(Roles = WebConstants.ModeratorRole)]
        public IActionResult Delete(int id, int publicationId)
        {
            return View();
        }

        [ActionName(nameof(Delete))]
        [Authorize(Roles = WebConstants.ModeratorRole)]
        [HttpPost]
        public IActionResult DeletePost(int id, int publicationId)
        {
            bool success = this.commentService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(WebConstants.SuccessfullEntityOperation, Comment, WebConstants.Deleted));

            return RedirectToAction(
                nameof(PublicationsController.Details),
                Publications,
                new { area = string.Empty, id = publicationId });
        }
    }
}