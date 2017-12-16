namespace StarStuff.Test.Web.Areas.Moderator
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Services.Moderator;
    using StarStuff.Services.Moderator.Models.Telescopes;
    using StarStuff.Web.Areas.Moderator.Controllers;
    using StarStuff.Web.Infrastructure;
    using System;
    using System.Linq;
    using Xunit;

    public class TelescopesControllerTest : BaseAreaTest
    {
        private const string Telescope = "Telescope";
        private const string Telescopes = "Telescopes";

        [Fact]
        public void TelescopesController_ShouldBeOnlyForModerators()
        {
            // Arrange
            Type controller = typeof(TelescopesController);

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
        public void TelescopesController_ShouldBeInModeratorsArea()
        {
            // Arrange
            Type controller = typeof(TelescopesController);

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
            TelescopesController telescopesController = new TelescopesController(null);

            // Act
            IActionResult result = telescopesController.Create();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void CreatePost_WithExistingTelescopeName_ShouldReturnView()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            telescopeService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();
            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = tempData.Object;

            // Act
            IActionResult result = telescopesController.Create(formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<TelescopeFormServiceModel>();

            TelescopeFormServiceModel returnModel = model.As<TelescopeFormServiceModel>();
            this.AssertTelescopes(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Telescope));
        }

        [Fact]
        public void CreatePost_WithNotSuccessfullyCreatedTelescope_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            telescopeService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            telescopeService
                .Setup(s => s.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<string>()))
                .Returns(-1);

            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();
            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = telescopesController.Create(formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void CreatePost_WithSuccessfullyCreatedTelescope_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            const int telescopeId = 1;

            telescopeService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            telescopeService
                .Setup(s => s.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<string>()))
                .Returns(telescopeId);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();
            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = tempData.Object;

            // Act
            IActionResult result = telescopesController.Create(formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(telescopeId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Telescope, Added));
        }

        [Fact]
        public void EditGet_WithNotExistingTelescopeId_ShouldRetunBadRequest()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            TelescopeFormServiceModel formModel = null;

            telescopeService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = telescopesController.Edit(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditGet_WithExistingTelescopeId_ShouldRetunView()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();

            telescopeService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = telescopesController.Edit(1);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<TelescopeFormServiceModel>();

            TelescopeFormServiceModel returnModel = model.As<TelescopeFormServiceModel>();
            this.AssertTelescopes(formModel, returnModel);
        }

        [Fact]
        public void EditPost_WithNotExistingTelescopeId_ShouldRetunBadRequest()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            TelescopeFormServiceModel formModel = null;

            telescopeService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = Mock.Of<ITempDataDictionary>();

            // Act
            IActionResult result = telescopesController.Edit(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithExistingTelescopeName_ShouldReturnView()
        {
            // Arranges
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            telescopeService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            telescopeService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns("New Name");

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();
            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);
            telescopesController.TempData = tempData.Object;

            // Act
            IActionResult result = telescopesController.Edit(1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<TelescopeFormServiceModel>();

            TelescopeFormServiceModel returnModel = model.As<TelescopeFormServiceModel>();
            this.AssertTelescopes(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Telescope));
        }

        [Fact]
        public void EditPost_WithNotSuccessfullEdit_ShouldReturnBadRequest()
        {
            // Arranges
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();

            telescopeService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            telescopeService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            telescopeService
                .Setup(s => s.Edit(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<string>()))
                .Returns(false);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);

            // Act
            IActionResult result = telescopesController.Edit(1, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithSuccessfullEdit_ShouldReturnRedirectResult()
        {
            // Arranges
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            TelescopeFormServiceModel formModel = this.GetTelescopeFormModel();

            const int telescopeId = 1;

            telescopeService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            telescopeService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            telescopeService
                .Setup(s => s.Edit(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<string>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);

            telescopesController.TempData = tempData.Object;

            // Act
            IActionResult result = telescopesController.Edit(telescopeId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(telescopeId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Telescope, Edited));
        }

        private void AssertTelescopes(TelescopeFormServiceModel expected, TelescopeFormServiceModel actual)
        {
            actual.Name.Should().Be(expected.Name);
            actual.Location.Should().Be(expected.Location);
            actual.Description.Should().Be(expected.Description);
            actual.MirrorDiameter.Should().Be(expected.MirrorDiameter);
            actual.ImageUrl.Should().Be(expected.ImageUrl);
        }

        private void AssertRedirect(int telescopeId, RedirectToActionResult result)
        {
            result.ActionName.Should().Be(Details);
            result.ControllerName.Should().Be(Telescopes);
            result.RouteValues[Id] = telescopeId;
            result.RouteValues[Area] = string.Empty;
        }

        private TelescopeFormServiceModel GetTelescopeFormModel()
        {
            return new TelescopeFormServiceModel
            {
                Name = "Test Name",
                Location = "Test Location",
                Description = "Test Description",
                MirrorDiameter = 22.5,
                ImageUrl = "Test Image Url"
            };
        }
    }
}