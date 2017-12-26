namespace StarStuff.Test.Web.Areas.Moderator
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Services.Areas.Moderator;
    using StarStuff.Services.Areas.Moderator.Models.Telescopes;
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
            Assert.NotNull(attribute);
            Assert.Equal(WebConstants.ModeratorRole, attribute.Roles);
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
            Assert.NotNull(attribute);
            Assert.Equal(WebConstants.ModeratorArea, attribute.RouteValue);
        }

        [Fact]
        public void CreateGet_ShouldReturnView()
        {
            // Arrange
            TelescopesController telescopesController = new TelescopesController(null);

            // Act
            IActionResult result = telescopesController.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<TelescopeFormServiceModel>(model);
            TelescopeFormServiceModel returnModel = model as TelescopeFormServiceModel;
            this.AssertTelescopes(formModel, returnModel);
            Assert.Equal(string.Format(WebConstants.EntryExists, Telescope), errorMessage);
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(telescopeId, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Telescope, WebConstants.Added), successmessage);
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<TelescopeFormServiceModel>(model);
            TelescopeFormServiceModel returnModel = model as TelescopeFormServiceModel;
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<TelescopeFormServiceModel>(model);
            TelescopeFormServiceModel returnModel = model as TelescopeFormServiceModel;
            this.AssertTelescopes(formModel, returnModel);
            Assert.Equal(string.Format(WebConstants.EntryExists, Telescope), errorMessage);
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(telescopeId, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Telescope, WebConstants.Edited), successmessage);
        }

        private void AssertTelescopes(TelescopeFormServiceModel expected, TelescopeFormServiceModel actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Location, actual.Location);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.MirrorDiameter, actual.MirrorDiameter);
            Assert.Equal(expected.ImageUrl, actual.ImageUrl);
        }

        private void AssertRedirect(int telescopeId, RedirectToActionResult result)
        {
            Assert.Equal(result.ActionName, Details);
            Assert.Equal(result.ControllerName, Telescopes);
            Assert.Equal(result.RouteValues[Id], telescopeId);
            Assert.Equal(result.RouteValues[Area], string.Empty);
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