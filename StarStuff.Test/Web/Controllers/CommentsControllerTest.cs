namespace StarStuff.Test.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Mocks;
    using Moq;
    using StarStuff.Data.Models;
    using StarStuff.Services;
    using StarStuff.Services.Models.Comments;
    using StarStuff.Web.Controllers;
    using StarStuff.Web.Infrastructure;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using Xunit;

    public class CommentsControllerTest : BaseGlobalControllerTest
    {
        private const string Comment = "Comment";
        private const string Details = "Details";
        private const string Publications = "Publications";
        private const string Id = "Id";
        private const string Area = "Area";
        private const string Page = "Page";

        [Fact]
        public void CommentsController_ShouldBeOnlyForAuthorizedUsers()
        {
            // Arrange
            Type controller = typeof(CommentsController);

            // Act
            AuthorizeAttribute attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void CreateGet_ShouldReturnView()
        {
            // Arrange
            CommentsController commentsController = new CommentsController(null, null);

            // Act
            IActionResult result = commentsController.Create(1, 1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreatePost_WithNotSuccessfullCreate_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;

            commentService
                .Setup(c => c.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(false);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object);

            // Act
            IActionResult result = commentsController.Create(1, 1, new CommentFormServiceModel { });

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreatePost_WithSuccessfullCreate_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            commentService
                .Setup(c => c.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object);
            commentsController.TempData = tempData.Object;

            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Create(publicationId, page, new CommentFormServiceModel { });

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(publicationId, page, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Comment, WebConstants.Added), successmessage);
        }

        [Fact]
        public void EditGet_WithUserNotInModeratorRoleAndNotOwningTheComment_ShouldReturnUnauthorized()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();

            commentService
                .Setup(c => c.CanEdit(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            claimsMock
                .Setup(c => c.IsInRole(It.IsAny<string>()))
                .Returns(false);

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            const int commentId = 1;
            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Edit(commentId, publicationId, page);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void EditGet_WithNotExistingCommentId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();

            CommentFormServiceModel fromModel = null;

            commentService
                .Setup(c => c.CanEdit(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            commentService
                .Setup(c => c.GetForm(It.IsAny<int>()))
                .Returns(fromModel);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            claimsMock
                .Setup(c => c.IsInRole(It.IsAny<string>()))
                .Returns(true);

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            const int commentId = 1;
            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Edit(commentId, publicationId, page);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EditGet_ShouldReturnView()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();

            CommentFormServiceModel formModel = this.GetCommentFormServiceModel();

            commentService
                .Setup(c => c.CanEdit(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            commentService
                .Setup(c => c.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            claimsMock
                .Setup(c => c.IsInRole(It.IsAny<string>()))
                .Returns(true);

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            const int commentId = 1;
            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Edit(commentId, publicationId, page);

            // Assert
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<CommentFormServiceModel>(model);
            CommentFormServiceModel returnModel = model as CommentFormServiceModel;
            this.AssertComment(formModel, returnModel);
        }

        [Fact]
        public void EditPost_WithUserNotInModeratorRoleAndNotOwningTheComment_ShouldReturnUnauthorized()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();

            commentService
                .Setup(c => c.CanEdit(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            claimsMock
                .Setup(c => c.IsInRole(It.IsAny<string>()))
                .Returns(false);

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            const int commentId = 1;
            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Edit(commentId, publicationId, page, new CommentFormServiceModel { });

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void EditPost_WithNotSuccessfullEdit_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();

            commentService
                .Setup(c => c.CanEdit(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            commentService
                .Setup(c => c.Edit(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(false);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            claimsMock
                .Setup(c => c.IsInRole(It.IsAny<string>()))
                .Returns(false);

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            const int commentId = 1;
            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Edit(commentId, publicationId, page, new CommentFormServiceModel { });

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EditPost_WithSuccessfullEdit_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;
            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            commentService
                .Setup(c => c.CanEdit(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            commentService
                .Setup(c => c.Edit(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            claimsMock
                .Setup(c => c.IsInRole(It.IsAny<string>()))
                .Returns(true);

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            CommentsController commentsController = new CommentsController(commentService.Object, userManager.Object)
            {
                TempData = tempData.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            const int commentId = 1;
            const int publicationId = 1;
            const int page = 1;

            // Act
            IActionResult result = commentsController.Edit(commentId, publicationId, page, new CommentFormServiceModel { });

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(publicationId, page, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Comment, WebConstants.Edited), successmessage);
        }

        [Fact]
        public void DeleteGet_ShouldBeOnlyForModerators()
        {
            // Arrange
            Type controller = typeof(CommentsController);
            MethodInfo action = controller.GetMethod(nameof(CommentsController.Delete));

            // Act
            AuthorizeAttribute attribute = action
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal(WebConstants.ModeratorRole, attribute.Roles);
        }

        [Fact]
        public void DeleteGet_ShouldReturnView()
        {
            // Arrange
            CommentsController commentsController = new CommentsController(null, null);

            // Act
            IActionResult result = commentsController.Delete(1, 1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DeletePost_ShouldBeOnlyForModerators()
        {
            // Arrange
            Type controller = typeof(CommentsController);
            MethodInfo action = controller.GetMethod(nameof(CommentsController.DeletePost));

            // Act
            AuthorizeAttribute attribute = action
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal(WebConstants.ModeratorRole, attribute.Roles);
        }

        [Fact]
        public void DeletePost_WithNotExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();

            commentService
                .Setup(c => c.Delete(It.IsAny<int>()))
                .Returns(false);

            CommentsController commentsController = new CommentsController(commentService.Object, null);

            // Act
            IActionResult result = commentsController.DeletePost(1, 1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeletePost_WithSuccessfullDelete_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            commentService
                .Setup(c => c.Delete(It.IsAny<int>()))
                .Returns(true);

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            const int publicationId = 1;

            CommentsController commentsController = new CommentsController(commentService.Object, null);
            commentsController.TempData = tempData.Object;

            // Act
            IActionResult result = commentsController.DeletePost(1, 1);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(publicationId, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Comment, WebConstants.Deleted), successmessage);
        }

        private void AssertComment(CommentFormServiceModel expected, CommentFormServiceModel actual)
        {
            Assert.Equal(expected.Content, actual.Content);
        }

        private CommentFormServiceModel GetCommentFormServiceModel()
        {
            return new CommentFormServiceModel
            {
                Content = "Test Content"
            };
        }

        private void AssertRedirect(int publicationId, RedirectToActionResult result)
        {
            Assert.Equal(result.ActionName, Details);
            Assert.Equal(result.ControllerName, Publications);
            Assert.Equal(result.RouteValues[Id], publicationId);
            Assert.Equal(result.RouteValues[Area], string.Empty);
        }

        private void AssertRedirect(int publicationId, int page, RedirectToActionResult result)
        {
            this.AssertRedirect(publicationId, result);
            Assert.Equal(result.RouteValues[Page], page);
        }
    }
}