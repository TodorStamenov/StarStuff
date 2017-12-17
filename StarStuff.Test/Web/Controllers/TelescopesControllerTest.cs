namespace StarStuff.Test.Web.Controllers
{
    using Moq;
    using System.Collections.Generic;
    using Xunit;
    using System.Linq;
    using StarStuff.Services.Moderator.Models.Telescopes;
    using FluentAssertions;
    using StarStuff.Web.Controllers;
    using StarStuff.Services.Moderator;
    using Microsoft.AspNetCore.Mvc;
    using StarStuff.Web.Models.Telescopes;
    using StarStuff.Data.Models;

    public class TelescopesControllerTest : BaseGlobalControllerTest
    {
        [Fact]
        public void Details_WithNotExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            TelescopeDetailsServiceModel detailsModel = null;

            telescopeService
                .Setup(t => t.Details(It.IsAny<int>()))
                .Returns(detailsModel);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);

            // Act
            IActionResult result = telescopesController.Details(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Details_WithExistingId_ShouldReturnView()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            TelescopeDetailsServiceModel detailsModel = this.GetTelescopeDetailsServiceModel();

            telescopeService
                .Setup(t => t.Details(It.IsAny<int>()))
                .Returns(detailsModel);

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);

            // Act
            IActionResult result = telescopesController.Details(1);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<TelescopeDetailsServiceModel>();

            TelescopeDetailsServiceModel returnModel = model.As<TelescopeDetailsServiceModel>();
            this.AssertTelescopeDetails(detailsModel, returnModel);
        }

        [Fact]
        public void All_ShouldReturnView()
        {
            // Arrange
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();
            ListTelescopesViewModel listModel = this.GetListTelescopesViewModel();

            telescopeService
                .Setup(t => t.Total())
                .Returns(20);

            telescopeService
                .Setup(t => t.All(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(this.GetListTelescopesServiceModel());

            TelescopesController telescopesController = new TelescopesController(telescopeService.Object);

            // Act
            IActionResult result = telescopesController.All(2);

            // Assert
            result.Should().BeOfType<ViewResult>();
            object model = result.As<ViewResult>().Model;

            model.Should().BeOfType<ListTelescopesViewModel>();

            ListTelescopesViewModel returnModel = model.As<ListTelescopesViewModel>();
            this.AssertPages(listModel, returnModel);
            this.AssertTelescopeCollection(listModel.Telescopes, returnModel.Telescopes);
        }

        private void AssertTelescopeDetails(TelescopeDetailsServiceModel expected, TelescopeDetailsServiceModel actual)
        {
            this.AssertTelescopes(expected, actual);
            actual.MirrorDiameter.Should().Be(expected.MirrorDiameter);
        }

        private void AssertTelescopes(ListTelescopesServiceModel expected, ListTelescopesServiceModel actual)
        {
            actual.Id.Should().Be(expected.Id);
            actual.Name.Should().Be(expected.Name);
            actual.Description.Should().Be(expected.Description);
            actual.ImageUrl.Should().Be(expected.ImageUrl);
        }

        private void AssertTelescopeCollection(IEnumerable<ListTelescopesServiceModel> expected, IEnumerable<ListTelescopesServiceModel> actual)
        {
            this.AssertTelescopes(expected.First(), actual.First());
            this.AssertTelescopes(expected.Last(), actual.Last());
        }

        private TelescopeDetailsServiceModel GetTelescopeDetailsServiceModel()
        {
            return new TelescopeDetailsServiceModel
            {
                Id = 1,
                Name = "Test Name",
                Location = "Test Location",
                Description = "Test Description",
                MirrorDiameter = 22.5,
                ImageUrl = "Test Image Url"
            };
        }

        private ListTelescopesViewModel GetListTelescopesViewModel()
        {
            return new ListTelescopesViewModel
            {
                CurrentPage = 2,
                TotalPages = 2,
                Telescopes = this.GetListTelescopesServiceModel()
            };
        }

        private IEnumerable<ListTelescopesServiceModel> GetListTelescopesServiceModel()
        {
            List<Telescope> telescopes = new List<Telescope>();

            for (int i = 1; i <= 20; i++)
            {
                telescopes.Add(new Telescope
                {
                    Id = i,
                    Name = $"Telescope Name {i}",
                    Description = $"Telescope Description {i}",
                    ImageUrl = $"Telescope Image Url {i}",
                    MirrorDiameter = 22.5
                });
            }

            return telescopes
                .OrderBy(t => t.MirrorDiameter)
                .Select(t => new ListTelescopesServiceModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    ImageUrl = t.ImageUrl
                })
                .ToList();
        }
    }
}