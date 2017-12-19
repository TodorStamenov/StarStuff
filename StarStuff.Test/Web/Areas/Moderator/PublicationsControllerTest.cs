namespace StarStuff.Test.Web.Areas.Moderator
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Data.Models;
    using StarStuff.Services.Astronomer;
    using StarStuff.Services.Astronomer.Models.Discoveries;
    using StarStuff.Services.Moderator;
    using StarStuff.Services.Moderator.Models.Publications;
    using StarStuff.Test.Mocks;
    using StarStuff.Web.Areas.Moderator.Controllers;
    using StarStuff.Web.Areas.Moderator.Models.Publications;
    using StarStuff.Web.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Xunit;

    public class PublicationsControllerTest : BaseAreaTest
    {
        private const string Publication = "Publication";
        private const string Publications = "Publications";

        [Fact]
        public void PublicationsController_ShouldBeOnlyForModerators()
        {
            // Arrange
            Type controller = typeof(PublicationsController);

            // Act
            AuthorizeAttribute attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.Roles.Should().Be(WebConstants.ModeratorRole);
        }

        [Fact]
        public void PublicationsController_ShouldBeInModeratorsArea()
        {
            // Arrange
            Type controller = typeof(PublicationsController);

            // Act
            AreaAttribute attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.RouteValue.Should().Be(WebConstants.ModeratorArea);
        }

        [Fact]
        public void CreateGet_ShouldReturnCorrectViewModel()
        {
            // Arrange
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            const int journalId = 1;

            discoveryService
                .Setup(d => d.DiscoveryDropdown(It.IsAny<int>()))
                .Returns(this.GetDiscoveriesDropdown());

            journalService
                .Setup(j => j.GetName(It.IsAny<int>()))
                .Returns("Journal Name");

            PublicationsController publicationsController = new PublicationsController(null, discoveryService.Object, journalService.Object, null);

            // Act
            IActionResult result = publicationsController.Create(journalId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PublicationFormViewModel>();

            PublicationFormViewModel returnModel = model.As<PublicationFormViewModel>();

            returnModel.JournalName.Should().Be("Journal Name");
            this.AssertDiscoveriesSelectList(returnModel.Discoveries);
        }

        [Fact]
        public void CreatePost_WithNotValidModelState_ShouldReturnCorrectViewModel()
        {
            // Arrange
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            PublicationFormViewModel formModel = this.GetPublicationFormViewModel();

            const int journalId = 1;

            discoveryService
                .Setup(d => d.DiscoveryDropdown(It.IsAny<int>()))
                .Returns(this.GetDiscoveriesDropdown());

            journalService
               .Setup(j => j.GetName(It.IsAny<int>()))
               .Returns(formModel.JournalName);

            PublicationsController publicationsController = new PublicationsController(null, discoveryService.Object, journalService.Object, null);
            publicationsController.ModelState.AddModelError(string.Empty, "Error");

            // Act
            IActionResult result = publicationsController.Create(journalId, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PublicationFormViewModel>();

            PublicationFormViewModel returnModel = model.As<PublicationFormViewModel>();
            this.AssertPublicationFormViewModel(formModel, returnModel);
        }

        [Fact]
        public void CreatePost_WithExistingPublicationTitle_ShouldReturnCorrectViewModel()
        {
            // Arrange
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            PublicationFormViewModel formModel = this.GetPublicationFormViewModel();

            const int journalId = 1;

            discoveryService
                .Setup(d => d.DiscoveryDropdown(It.IsAny<int>()))
                .Returns(this.GetDiscoveriesDropdown());

            journalService
               .Setup(j => j.GetName(It.IsAny<int>()))
               .Returns(formModel.JournalName);

            publicationService
               .Setup(p => p.TitleExists(It.IsAny<string>()))
               .Returns(true);

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, discoveryService.Object, journalService.Object, null);
            publicationsController.TempData = tempData.Object;

            // Act
            IActionResult result = publicationsController.Create(journalId, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PublicationFormViewModel>();

            PublicationFormViewModel returnModel = model.As<PublicationFormViewModel>();
            this.AssertPublicationFormViewModel(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Publication));
        }

        [Fact]
        public void CreatePost_WithExistingPublicationFromJournalForDiscovery_ShouldReturnCorrectViewModel()
        {
            // Arrange
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            PublicationFormViewModel formModel = this.GetPublicationFormViewModel();

            const int journalId = 1;

            discoveryService
                .Setup(d => d.DiscoveryDropdown(It.IsAny<int>()))
                .Returns(this.GetDiscoveriesDropdown());

            journalService
               .Setup(j => j.GetName(It.IsAny<int>()))
               .Returns(formModel.JournalName);

            publicationService
               .Setup(p => p.TitleExists(It.IsAny<string>()))
               .Returns(false);

            publicationService
               .Setup(p => p.Exists(It.IsAny<int>(), It.IsAny<int>()))
               .Returns(true);

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, discoveryService.Object, journalService.Object, null);
            publicationsController.TempData = tempData.Object;

            // Act
            IActionResult result = publicationsController.Create(journalId, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PublicationFormViewModel>();

            PublicationFormViewModel returnModel = model.As<PublicationFormViewModel>();
            this.AssertPublicationFormViewModel(formModel, returnModel);
            errorMessage.Should().Be("Publication from this Journal for this Discovery already exists!");
        }

        [Fact]
        public void CreatePost_WithNotSuccessfullyCreatedPublication_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;

            const int journalId = 1;

            publicationService
                .Setup(p => p.TitleExists(It.IsAny<string>()))
                .Returns(false);

            publicationService
                .Setup(p => p.Exists(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            publicationService
                .Setup(p => p.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(-1);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            PublicationFormViewModel formModel = this.GetPublicationFormViewModel();
            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, userManager.Object);

            // Act
            IActionResult result = publicationsController.Create(journalId, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void CreatePost_WithSuccessfullyCreatedPublication_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            Mock<UserManager<User>> userManager = UserManagerMock.New;

            const int journalId = 1;

            publicationService
                .Setup(p => p.TitleExists(It.IsAny<string>()))
                .Returns(false);

            publicationService
                .Setup(p => p.Exists(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            publicationService
                .Setup(p => p.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(journalId);

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            PublicationFormViewModel formModel = this.GetPublicationFormViewModel();
            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, userManager.Object);
            publicationsController.TempData = tempData.Object;

            // Act
            IActionResult result = publicationsController.Create(journalId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(journalId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Publication, Added));
        }

        [Fact]
        public void EditGet_WithNotExistingPublicationId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();

            const int journalId = 1;
            PublicationFormServiceModel formModel = null;

            publicationService
                .Setup(p => p.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, null);

            // Act
            IActionResult result = publicationsController.Edit(journalId);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditGet_WithExistingPublicationId_ShouldReturnView()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();

            const int journalId = 1;
            PublicationFormServiceModel formModel = this.GetPublicationFormServiceModel();

            publicationService
                .Setup(p => p.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, null);

            // Act
            IActionResult result = publicationsController.Edit(journalId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PublicationFormServiceModel>();

            PublicationFormServiceModel returnModel = model.As<PublicationFormServiceModel>();
            this.AssertPublications(formModel, returnModel);
        }

        [Fact]
        public void EditPost_WithNotExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();

            const int journalId = 1;
            PublicationFormServiceModel formModel = this.GetPublicationFormServiceModel();

            string title = null;

            publicationService
                .Setup(p => p.GetTitle(It.IsAny<int>()))
                .Returns(title);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, null);

            // Act
            IActionResult result = publicationsController.Edit(journalId, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithNewExistingTitle_ShouldReturnCorrectView()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            const int journalId = 1;
            PublicationFormServiceModel formModel = this.GetPublicationFormServiceModel();

            publicationService
                .Setup(p => p.GetTitle(It.IsAny<int>()))
                .Returns(formModel.Title);

            formModel.Title = "New Title";

            publicationService
                .Setup(p => p.TitleExists(It.IsAny<string>()))
                .Returns(true);

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, null);
            publicationsController.TempData = tempData.Object;

            // Act
            IActionResult result = publicationsController.Edit(journalId, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PublicationFormServiceModel>();

            PublicationFormServiceModel returnModel = model.As<PublicationFormServiceModel>();
            this.AssertPublications(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Publication));
        }

        [Fact]
        public void EditPost_WithNotSuccessfullEdit_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();

            const int journalId = 1;
            PublicationFormServiceModel formModel = this.GetPublicationFormServiceModel();

            publicationService
                .Setup(p => p.GetTitle(It.IsAny<int>()))
                .Returns(formModel.Title);

            publicationService
                .Setup(p => p.TitleExists(It.IsAny<string>()))
                .Returns(false);

            publicationService
                .Setup(p => p.Edit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, null);

            // Act
            IActionResult result = publicationsController.Edit(journalId, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithSuccessfullEdit_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            const int journalId = 1;
            PublicationFormServiceModel formModel = this.GetPublicationFormServiceModel();

            publicationService
                .Setup(p => p.GetTitle(It.IsAny<int>()))
                .Returns("Fake Title");

            publicationService
                .Setup(p => p.TitleExists(It.IsAny<string>()))
                .Returns(false);

            publicationService
                .Setup(p => p.Edit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            PublicationsController publicationsController = new PublicationsController(publicationService.Object, null, null, null);
            publicationsController.TempData = tempData.Object;

            // Act
            IActionResult result = publicationsController.Edit(journalId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(journalId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Publication, Edited));
        }

        private void AssertPublications(PublicationFormServiceModel expected, PublicationFormServiceModel actual)
        {
            actual.Title.Should().Be(expected.Title);
            actual.Content.Should().Be(expected.Content);
        }

        private void AssertPublicationFormViewModel(PublicationFormViewModel expected, PublicationFormViewModel actual)
        {
            actual.JournalName.Should().Be(expected.JournalName);
            actual.DiscoveryId.Should().Be(expected.DiscoveryId);
            this.AssertPublications(expected.Publication, actual.Publication);
            this.AssertDiscoveriesSelectList(actual.Discoveries);
        }

        private void AssertRedirect(int publicationId, RedirectToActionResult result)
        {
            result.ActionName.Should().Be(Details);
            result.ControllerName.Should().Be(Publications);
            result.RouteValues[Id].Should().Be(publicationId);
            result.RouteValues[Area].Should().Be(string.Empty);
        }

        private void AssertDiscoveriesSelectList(IEnumerable<SelectListItem> discoveries)
        {
            discoveries.Should().Match(items => items.Count() == 2);
            discoveries.First().Should().Match(d => d.As<SelectListItem>().Value == this.GetDiscoveriesDropdown().First().Id.ToString());
            discoveries.First().Should().Match(d => d.As<SelectListItem>().Text == this.GetDiscoveriesDropdown().First().StarSystem);
            discoveries.Last().Should().Match(d => d.As<SelectListItem>().Value == this.GetDiscoveriesDropdown().Last().Id.ToString());
            discoveries.Last().Should().Match(d => d.As<SelectListItem>().Text == this.GetDiscoveriesDropdown().Last().StarSystem);
        }

        private PublicationFormServiceModel GetPublicationFormServiceModel()
        {
            return new PublicationFormServiceModel
            {
                Title = "Publication Title",
                Content = "Publication Content"
            };
        }

        private PublicationFormViewModel GetPublicationFormViewModel()
        {
            return new PublicationFormViewModel
            {
                JournalName = "Journal Name",
                DiscoveryId = 1,
                Publication = this.GetPublicationFormServiceModel()
            };
        }

        private IEnumerable<DiscoveryServiceModel> GetDiscoveriesDropdown()
        {
            return new List<DiscoveryServiceModel>
            {
                new DiscoveryServiceModel { Id = 1, StarSystem = "First StarSystem" },
                new DiscoveryServiceModel { Id = 2, StarSystem = "Second StarSystem" }
            };
        }
    }
}