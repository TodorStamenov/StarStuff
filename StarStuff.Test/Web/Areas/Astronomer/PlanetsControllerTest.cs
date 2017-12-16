namespace StarStuff.Test.Web.Areas.Astronomer
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Services.Astronomer;
    using StarStuff.Services.Astronomer.Models.Planets;
    using StarStuff.Web.Areas.Astronomer.Controllers;
    using StarStuff.Web.Infrastructure;
    using System;
    using System.Linq;
    using Xunit;

    public class PlanetsControllerTest : BaseAreaTest
    {
        private const string Planet = "Planet";

        [Fact]
        public void PlanetsController_ShouldBeOnlyForAstronomers()
        {
            // Arrange
            Type controller = typeof(PlanetsController);

            // Act
            AuthorizeAttribute attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.Roles.Should().Be(WebConstants.AstronomerRole);
        }

        [Fact]
        public void PlanetsController_ShouldBeInAstronomersArea()
        {
            // Arrange
            Type controller = typeof(PlanetsController);

            // Act
            AreaAttribute attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.RouteValue.Should().Be(WebConstants.AstronomerArea);
        }

        [Fact]
        public void CreateGet_ShouldReturnView()
        {
            // Arrange
            PlanetsController planetsController = new PlanetsController(null);

            // Act
            IActionResult result = planetsController.Create();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void CreatePost_WithExistingPlanetName_ShouldReturnView()
        {
            // Arrange
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();

            planetService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            PlanetFormServiceModel formModel = this.GetPlanetFormModel();
            PlanetsController planetsController = new PlanetsController(planetService.Object);
            planetsController.TempData = tempData.Object;

            // Act
            IActionResult result = planetsController.Create(1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PlanetFormServiceModel>();

            PlanetFormServiceModel returnModel = model.As<PlanetFormServiceModel>();
            this.AssertPlanets(formModel, returnModel);

            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Planet));
        }

        [Fact]
        public void CreatePost_WithNotSuccessfullyCreatedPlanet_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();

            planetService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            planetService
                .Setup(s => s.Create(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);

            PlanetsController planetsController = new PlanetsController(planetService.Object);
            planetsController.TempData = Mock.Of<ITempDataDictionary>();
            PlanetFormServiceModel formModel = this.GetPlanetFormModel();

            // Act
            IActionResult result = planetsController.Create(1, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void CreatePost_WithSuccessfullyCreatedPlanet_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();
            const int discoveryId = 1;

            planetService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            planetService
                .Setup(s => s.Create(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            PlanetFormServiceModel formModel = this.GetPlanetFormModel();
            PlanetsController planetsController = new PlanetsController(planetService.Object);
            planetsController.TempData = tempData.Object;

            // Act
            IActionResult result = planetsController.Create(discoveryId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(discoveryId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Planet, Added));
        }

        [Fact]
        public void EditGet_WithExistingId_ShouldReturnView()
        {
            // Arrange
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();
            PlanetFormServiceModel formModel = this.GetPlanetFormModel();

            planetService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            PlanetsController planetsController = new PlanetsController(planetService.Object);

            // Act
            IActionResult result = planetsController.Edit(1, 1);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PlanetFormServiceModel>();

            PlanetFormServiceModel returnModel = model.As<PlanetFormServiceModel>();
            this.AssertPlanets(formModel, returnModel);
        }

        [Fact]
        public void EditGet_WithNotExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();
            PlanetFormServiceModel model = null;

            planetService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(model);

            PlanetsController planetsController = new PlanetsController(planetService.Object);

            // Act
            IActionResult result = planetsController.Edit(1, 1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithExistingPlnaetName_ShouldReturnView()
        {
            // Arranges
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();

            planetService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            planetService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns("New Name");

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            PlanetFormServiceModel formModel = this.GetPlanetFormModel();
            PlanetsController planetsController = new PlanetsController(planetService.Object);
            planetsController.TempData = tempData.Object;

            // Act
            IActionResult result = planetsController.Edit(1, 1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<PlanetFormServiceModel>();

            PlanetFormServiceModel returnModel = model.As<PlanetFormServiceModel>();
            this.AssertPlanets(formModel, returnModel);
            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Planet));
        }

        [Fact]
        public void EditPost_WithNotSuccessfullEdit_ShouldReturnBadRequest()
        {
            // Arranges
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();

            PlanetFormServiceModel formModel = this.GetPlanetFormModel();

            planetService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            planetService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            planetService
                .Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);

            PlanetsController planetsController = new PlanetsController(planetService.Object);

            // Act
            IActionResult result = planetsController.Edit(1, 1, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithSuccessfullEdit_ShouldReturnRedirectResult()
        {
            // Arranges
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();
            PlanetFormServiceModel formModel = this.GetPlanetFormModel();

            const int discoveryId = 1;

            planetService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            planetService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            planetService
                .Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            PlanetsController planetsController = new PlanetsController(planetService.Object);
            planetsController.TempData = tempData.Object;

            // Act
            IActionResult result = planetsController.Edit(1, discoveryId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(discoveryId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Planet, Edited));
        }

        [Fact]
        public void DeleteGet_ShouldReturnView()
        {
            // Arranges
            PlanetsController planetsController = new PlanetsController(null);

            // Act
            IActionResult result = planetsController.Delete(1, 1);

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void DeletePost_WithSuccessfullDelete_ShouldReturnRedirectResult()
        {
            // Arranges
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();

            planetService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            PlanetsController planetsController = new PlanetsController(planetService.Object);

            planetsController.TempData = tempData.Object;

            const int discoveryId = 1;

            // Act
            IActionResult result = planetsController.DeletePost(1, discoveryId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(discoveryId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Planet, Deleted));
        }

        [Fact]
        public void DeletePost_WithNotSuccessfullDelete_ShouldReturnBadRequest()
        {
            // Arranges
            Mock<IPlanetService> planetService = new Mock<IPlanetService>();

            planetService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .Returns(false);

            PlanetsController planetsController = new PlanetsController(planetService.Object);

            // Act
            IActionResult result = planetsController.DeletePost(1, 1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        private void AssertPlanets(PlanetFormServiceModel expected, PlanetFormServiceModel actual)
        {
            actual.Name.Should().Be(expected.Name);
            actual.Mass.Should().Be(expected.Mass);
        }

        private void AssertRedirect(int discoveryId, RedirectToActionResult result)
        {
            result.ActionName.Should().Be(nameof(DiscoveriesController.Details));
            result.ControllerName.Should().Be(Discoveries);
            result.RouteValues[Id] = discoveryId;
            result.RouteValues[Area] = WebConstants.AstronomerArea;
        }

        private PlanetFormServiceModel GetPlanetFormModel()
        {
            return new PlanetFormServiceModel { Name = "Test Name", Mass = 22.5 };
        }
    }
}