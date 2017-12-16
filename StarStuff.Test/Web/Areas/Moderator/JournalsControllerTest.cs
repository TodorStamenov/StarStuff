namespace StarStuff.Test.Web.Areas.Moderator
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Services.Moderator;
    using StarStuff.Services.Moderator.Models.Journals;
    using StarStuff.Web.Areas.Moderator.Controllers;
    using StarStuff.Web.Infrastructure;
    using System;
    using System.Linq;
    using Xunit;

    public class JournalsControllerTest : BaseAreaTest
    {
        private const string Journal = "Journal";
        private const string Journals = "Journals";

        [Fact]
        public void JournalsController_ShouldBeOnlyForModerators()
        {
            // Arrange
            Type controller = typeof(JournalsController);

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
        public void JournalsController_ShouldBeInModeratorsArea()
        {
            // Arrange
            Type controller = typeof(JournalsController);

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
        public void CreateGet_ShouldReturnView()
        {
            // Arrange
            JournalsController journalsController = new JournalsController(null);

            // Act
            IActionResult result = journalsController.Create();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void CreatePost_WithExistingJournalName_ShouldReturnView()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            journalService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            JournalFormServiceModel formModel = this.GetJournalFormModel();
            JournalsController journalsController = new JournalsController(journalService.Object);
            journalsController.TempData = tempData.Object;

            // Act
            IActionResult result = journalsController.Create(formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<JournalFormServiceModel>();

            JournalFormServiceModel returnModel = model.As<JournalFormServiceModel>();
            this.AssertJournals(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Journal));
        }

        [Fact]
        public void CreatePost_WithNotSuccessfullyCreatedJournal_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            journalService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            journalService
                .Setup(s => s.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(-1);

            JournalFormServiceModel formModel = this.GetJournalFormModel();
            JournalsController journalsController = new JournalsController(journalService.Object);

            journalsController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = journalsController.Create(formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void CreatePost_WithSuccessfullyCreatedJournal_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            const int journalId = 1;

            journalService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            journalService
                .Setup(s => s.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(journalId);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            JournalFormServiceModel formModel = this.GetJournalFormModel();
            JournalsController journalsController = new JournalsController(journalService.Object);
            journalsController.TempData = tempData.Object;

            // Act
            IActionResult result = journalsController.Create(formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(journalId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Journal, Added));
        }

        [Fact]
        public void EditGet_WithNotExistingJournalId_ShouldRetunBadRequest()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            JournalFormServiceModel formModel = null;

            journalService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            JournalsController journalsController = new JournalsController(journalService.Object);
            journalsController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = journalsController.Edit(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditGet_WithExistingJournalId_ShouldRetunView()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            JournalFormServiceModel formModel = this.GetJournalFormModel();

            journalService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            JournalsController journalsController = new JournalsController(journalService.Object);
            journalsController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = journalsController.Edit(1);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<JournalFormServiceModel>();

            JournalFormServiceModel returnModel = model.As<JournalFormServiceModel>();
            this.AssertJournals(formModel, returnModel);
        }

        [Fact]
        public void EditPost_WithNotExistingJournalId_ShouldRetunBadRequest()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            JournalFormServiceModel formModel = null;

            journalService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            JournalsController journalsController = new JournalsController(journalService.Object);
            journalsController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = journalsController.Edit(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithExistingJournalName_ShouldReturnView()
        {
            // Arranges
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            journalService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            journalService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns("New Name");

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            JournalFormServiceModel formModel = this.GetJournalFormModel();
            JournalsController journalsController = new JournalsController(journalService.Object);
            journalsController.TempData = tempData.Object;

            // Act
            IActionResult result = journalsController.Edit(1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<JournalFormServiceModel>();

            JournalFormServiceModel returnModel = model.As<JournalFormServiceModel>();
            this.AssertJournals(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Journal));
        }

        [Fact]
        public void EditPost_WithNotSuccessfullEdit_ShouldReturnBadRequest()
        {
            // Arranges
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            JournalFormServiceModel formModel = this.GetJournalFormModel();

            journalService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            journalService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            journalService
                .Setup(s => s.Edit(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(false);

            JournalsController journalsController = new JournalsController(journalService.Object);

            // Act
            IActionResult result = journalsController.Edit(1, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithSuccessfullEdit_ShouldReturnRedirectResult()
        {
            // Arranges
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            JournalFormServiceModel formModel = this.GetJournalFormModel();

            const int journalId = 1;

            journalService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            journalService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            journalService
                .Setup(s => s.Edit(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            JournalsController journalsController = new JournalsController(journalService.Object);

            journalsController.TempData = tempData.Object;

            // Act
            IActionResult result = journalsController.Edit(journalId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(journalId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Journal, Edited));
        }

        private void AssertJournals(JournalFormServiceModel expected, JournalFormServiceModel actual)
        {
            actual.Name.Should().Be(expected.Name);
            actual.Description.Should().Be(expected.Description);
            actual.ImageUrl.Should().Be(expected.ImageUrl);
        }

        private void AssertRedirect(int journalId, RedirectToActionResult result)
        {
            result.ActionName.Should().Be(Details);
            result.ControllerName.Should().Be(Journals);
            result.RouteValues[Id] = journalId;
            result.RouteValues[Area] = string.Empty;
        }

        private JournalFormServiceModel GetJournalFormModel()
        {
            return new JournalFormServiceModel
            {
                Name = "Test Name",
                Description = "Test Description",
                ImageUrl = "Test Image Url"
            };
        }
    }
}