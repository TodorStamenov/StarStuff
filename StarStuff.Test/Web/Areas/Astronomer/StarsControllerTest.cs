namespace StarStuff.Test.Web.Areas.Astronomer
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Data;
    using StarStuff.Services.Astronomer;
    using StarStuff.Services.Astronomer.Models.Stars;
    using StarStuff.Web.Areas.Astronomer.Controllers;
    using StarStuff.Web.Infrastructure;
    using System;
    using System.Linq;
    using Xunit;

    public class StarsControllerTest : BaseAreaTest
    {
        private const string Star = "Star";

        [Fact]
        public void StarsController_ShouldBeOnlyForAstronomers()
        {
            // Arrange
            Type controller = typeof(StarsController);

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
        public void StarsController_ShouldBeInAstronomersArea()
        {
            // Arrange
            Type controller = typeof(StarsController);

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
            StarsController starsController = new StarsController(null, null);

            // Act
            IActionResult result = starsController.Create();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void CreatePost_WithExistingStarName_ShouldReturnView()
        {
            // Arrange
            Mock<IStarService> starService = new Mock<IStarService>();

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            StarFormServiceModel formModel = this.GetStarFormModel();
            StarsController starsController = new StarsController(starService.Object, null);
            starsController.TempData = tempData.Object;

            // Act
            IActionResult result = starsController.Create(1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<StarFormServiceModel>();

            StarFormServiceModel returnModel = model.As<StarFormServiceModel>();
            this.AssertStars(formModel, returnModel);

            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Star));
        }

        [Fact]
        public void CreatePost_WithDiscoveryWithMoreThanThreeStars_ShouldReturnView()
        {
            // Arrange
            Mock<IStarService> starService = new Mock<IStarService>();
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            discoveryService
                .Setup(d => d.TotalStars(It.IsAny<int>()))
                .Returns(3);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            StarFormServiceModel formModel = this.GetStarFormModel();
            StarsController starsController = new StarsController(starService.Object, discoveryService.Object);
            starsController.TempData = tempData.Object;

            // Act
            IActionResult result = starsController.Create(1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<StarFormServiceModel>();

            StarFormServiceModel returnModel = model.As<StarFormServiceModel>();
            this.AssertStars(formModel, returnModel);

            errorMessage.Should().Be(string.Format(
                    DataConstants.DiscoveryConstants.MaxStarsPerDiscoveryErrorMessage,
                    DataConstants.DiscoveryConstants.MaxStarsPerDiscovery));
        }

        [Fact]
        public void CreatePost_WithNotSuccessfullyCreatedStar_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IStarService> starService = new Mock<IStarService>();
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            discoveryService
                .Setup(d => d.TotalStars(It.IsAny<int>()))
                .Returns(1);

            starService
                .Setup(s => s.Create(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);

            StarsController starsController = new StarsController(starService.Object, discoveryService.Object);
            starsController.TempData = Mock.Of<ITempDataDictionary>();
            StarFormServiceModel formModel = this.GetStarFormModel();

            // Act
            IActionResult result = starsController.Create(1, formModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void CreatePost_WithSuccessfullyCreatedStar_ShouldReturnRedirectResult()
        {
            // Arrange
            Mock<IStarService> starService = new Mock<IStarService>();
            Mock<IDiscoveryService> discoveryService = new Mock<IDiscoveryService>();

            const int discoveryId = 1;

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            discoveryService
                .Setup(d => d.TotalStars(It.IsAny<int>()))
                .Returns(1);

            starService
                .Setup(s => s.Create(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            StarFormServiceModel formModel = this.GetStarFormModel();
            StarsController starsController = new StarsController(starService.Object, discoveryService.Object);
            starsController.TempData = tempData.Object;

            // Act
            IActionResult result = starsController.Create(discoveryId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(discoveryId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Star, Added));
        }

        [Fact]
        public void EditGet_WithExistingId_ShouldReturnView()
        {
            // Arrange
            Mock<IStarService> starService = new Mock<IStarService>();
            StarFormServiceModel formModel = this.GetStarFormModel();

            starService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(formModel);

            StarsController starsController = new StarsController(starService.Object, null);

            // Act
            IActionResult result = starsController.Edit(1, 1);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<StarFormServiceModel>();

            StarFormServiceModel returnModel = model.As<StarFormServiceModel>();
            this.AssertStars(formModel, returnModel);
        }

        [Fact]
        public void EditGet_WithNotExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IStarService> starService = new Mock<IStarService>();
            StarFormServiceModel model = null;

            starService
                .Setup(s => s.GetForm(It.IsAny<int>()))
                .Returns(model);

            StarsController starsController = new StarsController(starService.Object, null);

            // Act
            IActionResult result = starsController.Edit(1, 1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithExistingStarName_ShouldReturnView()
        {
            // Arranges
            Mock<IStarService> starService = new Mock<IStarService>();

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);

            starService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns("New Name");

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string errorMessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            StarFormServiceModel formModel = this.GetStarFormModel();
            StarsController starsController = new StarsController(starService.Object, null);
            starsController.TempData = tempData.Object;

            // Act
            IActionResult result = starsController.Edit(1, 1, formModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<StarFormServiceModel>();

            StarFormServiceModel returnModel = model.As<StarFormServiceModel>();
            this.AssertStars(formModel, returnModel);

            errorMessage.Should().Be(string.Format(WebConstants.EntryExists, Star));
        }

        [Fact]
        public void EditPost_WithNotSuccessfullEdit_ShouldReturnView()
        {
            // Arranges
            Mock<IStarService> starService = new Mock<IStarService>();

            StarFormServiceModel model = this.GetStarFormModel();

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            starService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(model.Name);

            starService
                .Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);

            StarsController starsController = new StarsController(starService.Object, null);

            // Act
            IActionResult result = starsController.Edit(1, 1, model);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void EditPost_WithSuccessfullEdit_ShouldReturnRedirectResult()
        {
            // Arranges
            Mock<IStarService> starService = new Mock<IStarService>();
            StarFormServiceModel formModel = this.GetStarFormModel();

            const int discoveryId = 1;

            starService
                .Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);

            starService
                .Setup(s => s.GetName(It.IsAny<int>()))
                .Returns(formModel.Name);

            starService
                .Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            StarsController starsController = new StarsController(starService.Object, null);
            starsController.TempData = tempData.Object;

            // Act
            IActionResult result = starsController.Edit(1, discoveryId, formModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(discoveryId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Star, Edited));
        }

        [Fact]
        public void DeleteGet_ShouldReturnView()
        {
            // Arranges
            StarsController starsController = new StarsController(null, null);

            // Act
            IActionResult result = starsController.Delete(1, 1);

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void DeletePost_WithSuccessfullDelete_ShouldReturnRedirectResult()
        {
            // Arranges
            Mock<IStarService> starService = new Mock<IStarService>();

            starService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .Returns(true);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            string successmessage = null;

            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successmessage = message as string);

            StarsController starsController = new StarsController(starService.Object, null);
            starsController.TempData = tempData.Object;

            const int discoveryId = 1;

            // Act
            IActionResult result = starsController.DeletePost(1, discoveryId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            RedirectToActionResult redirectResult = result.As<RedirectToActionResult>();
            this.AssertRedirect(discoveryId, redirectResult);
            successmessage.Should().Be(string.Format(WebConstants.SuccessfullEntityOperation, Star, Deleted));
        }

        [Fact]
        public void DeletePost_WithNotSuccessfullDelete_ShouldReturnBadRequest()
        {
            // Arranges
            Mock<IStarService> starService = new Mock<IStarService>();

            starService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .Returns(false);

            StarsController starsController = new StarsController(starService.Object, null);

            // Act
            IActionResult result = starsController.DeletePost(1, 1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        private void AssertStars(StarFormServiceModel expected, StarFormServiceModel actual)
        {
            actual.Name.Should().Be(expected.Name);
            actual.Temperature.Should().Be(expected.Temperature);
        }

        private void AssertRedirect(int discoveryId, RedirectToActionResult result)
        {
            result.ActionName.Should().Be(nameof(DiscoveriesController.Details));
            result.ControllerName.Should().Be(Discoveries);
            result.RouteValues[Id] = discoveryId;
            result.RouteValues[Area] = WebConstants.AstronomerArea;
        }

        private StarFormServiceModel GetStarFormModel()
        {
            return new StarFormServiceModel { Name = "Test Name", Temperature = 300000 };
        }
    }
}