namespace StarStuff.Test.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using StarStuff.Services.Areas.Moderator;
    using StarStuff.Services.Areas.Moderator.Models.Journals;
    using StarStuff.Web.Controllers;
    using StarStuff.Web.Models.Journals;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class JournalsControllerTest : BaseGlobalControllerTest
    {
        [Fact]
        public void Details_WithNotExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            JournalDetailsServiceModel detailsModel = null;

            journalService
                .Setup(t => t.Details(It.IsAny<int>()))
                .Returns(detailsModel);

            JournalsController journalsController = new JournalsController(journalService.Object);

            // Act
            IActionResult result = journalsController.Details(1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Details_WithExistingId_ShouldReturnView()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            JournalDetailsServiceModel detailsModel = this.GetJournalDetailsServiceModel();

            journalService
                .Setup(t => t.Details(It.IsAny<int>()))
                .Returns(detailsModel);

            JournalsController journalsController = new JournalsController(journalService.Object);

            // Act
            IActionResult result = journalsController.Details(1);

            // Assert
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<JournalDetailsServiceModel>(model);
            JournalDetailsServiceModel returnModel = model as JournalDetailsServiceModel;
            this.AssertJournals(detailsModel, returnModel);
        }

        [Fact]
        public void All_ShouldReturnView()
        {
            // Arrange
            Mock<IJournalService> journalService = new Mock<IJournalService>();
            ListJournalsViewModel listModel = this.GetListJournalsViewModel();

            journalService
                .Setup(t => t.Total())
                .Returns(20);

            journalService
                .Setup(t => t.All(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(this.GetListJournalsServiceModel());

            JournalsController journalsController = new JournalsController(journalService.Object);

            // Act
            IActionResult result = journalsController.All(2);

            // Assert
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<ListJournalsViewModel>(model);
            ListJournalsViewModel returnModel = model as ListJournalsViewModel;
            this.AssertPages(listModel, returnModel);
            this.AssertJournals(listModel, returnModel);
        }

        private void AssertJournals(ListJournalsServiceModel expected, ListJournalsServiceModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ImageUrl, actual.ImageUrl);
        }

        private void AssertJournals(ListJournalsViewModel expected, ListJournalsViewModel actual)
        {
            this.AssertJournals(expected.Journals.First(), actual.Journals.First());
            this.AssertJournals(expected.Journals.Last(), actual.Journals.Last());
        }

        private JournalDetailsServiceModel GetJournalDetailsServiceModel()
        {
            return new JournalDetailsServiceModel
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description",
                ImageUrl = "Test Image Url"
            };
        }

        private ListJournalsViewModel GetListJournalsViewModel()
        {
            return new ListJournalsViewModel
            {
                CurrentPage = 2,
                TotalPages = 2,
                Journals = this.GetListJournalsServiceModel()
            };
        }

        private IEnumerable<ListJournalsServiceModel> GetListJournalsServiceModel()
        {
            List<ListJournalsServiceModel> journals = new List<ListJournalsServiceModel>();

            for (int i = 1; i <= 20; i++)
            {
                journals.Add(new ListJournalsServiceModel
                {
                    Id = i,
                    Name = $"Journal Name {i}",
                    Description = $"Journal Description {i}",
                    ImageUrl = $"Journal Image Url {i}"
                });
            }

            return journals.OrderBy(j => j.Name).ToList();
        }
    }
}