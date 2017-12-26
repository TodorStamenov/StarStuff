namespace StarStuff.Test.Web.Areas.Astronomer
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using StarStuff.Data;
    using StarStuff.Services.Areas.Astronomer;
    using StarStuff.Services.Areas.Astronomer.Models.Stars;
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
            Assert.NotNull(attribute);
            Assert.Equal(WebConstants.AstronomerRole, attribute.Roles);
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
            Assert.NotNull(attribute);
            Assert.Equal(WebConstants.AstronomerArea, attribute.RouteValue);
        }

        [Fact]
        public void CreateGet_ShouldReturnView()
        {
            // Arrange
            StarsController starsController = new StarsController(null, null);

            // Act
            IActionResult result = starsController.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<StarFormServiceModel>(model);
            StarFormServiceModel returnModel = model as StarFormServiceModel;
            this.AssertStars(formModel, returnModel);
            Assert.Equal(string.Format(WebConstants.EntryExists, Star), errorMessage);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<StarFormServiceModel>(model);
            StarFormServiceModel returnModel = model as StarFormServiceModel;
            this.AssertStars(formModel, returnModel);
            Assert.Equal(string.Format(
                    DataConstants.DiscoveryConstants.MaxStarsPerDiscoveryErrorMessage,
                    DataConstants.DiscoveryConstants.MaxStarsPerDiscovery),
                    errorMessage);
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(discoveryId, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Star, WebConstants.Added), successmessage);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<StarFormServiceModel>(model);
            StarFormServiceModel returnModel = model as StarFormServiceModel;
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<StarFormServiceModel>(model);
            StarFormServiceModel returnModel = model as StarFormServiceModel;
            this.AssertStars(formModel, returnModel);
            Assert.Equal(string.Format(WebConstants.EntryExists, Star), errorMessage);
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
            Assert.IsType<BadRequestResult>(result);
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
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(discoveryId, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Star, WebConstants.Edited), successmessage);
        }

        [Fact]
        public void DeleteGet_ShouldReturnView()
        {
            // Arranges
            StarsController starsController = new StarsController(null, null);

            // Act
            IActionResult result = starsController.Delete(1, 1);

            // Assert
            Assert.IsType<ViewResult>(result);
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
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            this.AssertRedirect(discoveryId, redirectResult);
            Assert.Equal(string.Format(WebConstants.SuccessfullEntityOperation, Star, WebConstants.Deleted), successmessage);
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
            Assert.IsType<BadRequestResult>(result);
        }

        private void AssertStars(StarFormServiceModel expected, StarFormServiceModel actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Temperature, actual.Temperature);
        }

        private void AssertRedirect(int discoveryId, RedirectToActionResult result)
        {
            Assert.Equal(nameof(DiscoveriesController.Details), result.ActionName);
            Assert.Equal(Discoveries, result.ControllerName);
            Assert.Equal(discoveryId, result.RouteValues[Id]);
            Assert.Equal(WebConstants.AstronomerArea, result.RouteValues[Area]);
        }

        private StarFormServiceModel GetStarFormModel()
        {
            return new StarFormServiceModel { Name = "Test Name", Temperature = 300000 };
        }
    }
}