namespace StarStuff.Test.Web.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using StarStuff.Data.Models;
    using StarStuff.Services;
    using StarStuff.Services.Areas.Moderator;
    using StarStuff.Services.Areas.Moderator.Models.Publications;
    using StarStuff.Services.Models.Comments;
    using StarStuff.Web.Controllers;
    using StarStuff.Web.Models.Publications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Xunit;

    public class PublicationsControllerTest : BaseGlobalControllerTest
    {
        [Fact]
        public void Details_WithNotValidId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();

            PublicationDetailsServiceModel detailsModel = null;

            const int publicationId = 1;

            commentService
                .Setup(c => c.Total(publicationId))
                .Returns(1);

            publicationService
                .Setup(p => p.Details(It.IsAny<int>()))
                .Returns(detailsModel);

            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(t => t.Identity.IsAuthenticated)
                .Returns(false);

            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();

            mockHttpContext
                .Setup(m => m.User)
                .Returns(claimsMock.Object);

            PublicationsController publicationsController =
                new PublicationsController(commentService.Object, null, null, publicationService.Object, null)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object
                    }
                };

            // Act
            IActionResult result = publicationsController.Details(publicationId, 1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Details_WithValidId_ShouldReturnView()
        {
            // Arrange
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();

            PublicationDetailsServiceModel detailsModel = this.GetPublicationDetailsServiceModel();

            const int publicationId = 1;

            commentService
                .Setup(c => c.Total(publicationId))
                .Returns(20);

            commentService
                .Setup(c => c.All(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int?>()))
                .Returns(this.GetComments());

            publicationService
                .Setup(p => p.Details(It.IsAny<int>()))
                .Returns(detailsModel);

            Mock<ClaimsPrincipal> claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(t => t.Identity.IsAuthenticated)
                .Returns(false);

            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsMock.Object);

            PublicationsController publicationsController =
                new PublicationsController(commentService.Object, null, null, publicationService.Object, null)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object
                    }
                };

            // Act
            IActionResult result = publicationsController.Details(publicationId, 2);

            // Assert
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<PublicationDetailsViewModel>(model);
            PublicationDetailsViewModel returnModel = model as PublicationDetailsViewModel;
            this.AssertPublicationDetailsViewModel(this.GetPublicationDetailsViewModel(), returnModel);
        }

        [Fact]
        public void All_ShouldReturnView()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            ListPublicationsViewModel listModel = this.GetListPublicationsViewModel();

            publicationService
                .Setup(p => p.Total())
                .Returns(20);

            publicationService
                .Setup(p => p.All(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(this.GetPublications());

            PublicationsController publicationsController =
                new PublicationsController(null, null, null, publicationService.Object, null);

            // Act
            IActionResult result = publicationsController.All(2);

            // Assert
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<ListPublicationsViewModel>(model);
            ListPublicationsViewModel returnModel = model as ListPublicationsViewModel;
            this.AssertPages(listModel, returnModel);
            this.AssertListPublicationsViewModel(listModel, returnModel);
        }

        [Fact]
        public void ByJournal_ShouldReturnView()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            ListPublicationsByJournalViewModel listModel = this.GetListPublicationsByJournalViewModel();

            publicationService
                .Setup(p => p.TotalByJournal(It.IsAny<int>()))
                .Returns(20);

            publicationService
                .Setup(p => p.AllByJournal(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(this.GetPublications());

            journalService
                .Setup(j => j.GetName(It.IsAny<int>()))
                .Returns("Journal Name");

            PublicationsController publicationsController =
                new PublicationsController(null, journalService.Object, null, publicationService.Object, null);

            // Act
            IActionResult result = publicationsController.ByJournal(1, 2);

            // Assert
            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<ListPublicationsByJournalViewModel>(model);
            ListPublicationsByJournalViewModel returnModel = model as ListPublicationsByJournalViewModel;
            this.AssertPages(listModel, returnModel);
            this.AssertListPublicationsByJournalViewModel(listModel, returnModel);
        }

        [Fact]
        public void ByJournal_WithNotExistingJournalId_ShouldReturnBadRequest()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<IJournalService> journalService = new Mock<IJournalService>();

            ListPublicationsByJournalViewModel listModel = this.GetListPublicationsByJournalViewModel();
            IEnumerable<ListPublicationsServiceModel> publications = null;

            publicationService
                .Setup(p => p.TotalByJournal(It.IsAny<int>()))
                .Returns(20);

            publicationService
                .Setup(p => p.AllByJournal(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(publications);

            journalService
                .Setup(j => j.GetName(It.IsAny<int>()))
                .Returns("Test Name");

            PublicationsController publicationsController =
                new PublicationsController(null, journalService.Object, null, publicationService.Object, null);

            // Act
            IActionResult result = publicationsController.ByJournal(1, 2);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ByTelescope_ShouldReturnView()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            ListPublicationsByTelescopeViewModel listModel = this.GetListPublicationsByTelescopeViewModel();

            publicationService
                .Setup(p => p.TotalByTelescope(It.IsAny<int>()))
                .Returns(20);

            publicationService
                .Setup(p => p.AllByTelescope(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(this.GetPublications());

            telescopeService
                .Setup(j => j.GetName(It.IsAny<int>()))
                .Returns("Journal Name");

            PublicationsController publicationsController =
                new PublicationsController(null, null, telescopeService.Object, publicationService.Object, null);

            // Act
            IActionResult result = publicationsController.ByTelescope(1, 2);

            // Assert

            Assert.IsType<ViewResult>(result);
            object model = (result as ViewResult).Model;
            Assert.IsType<ListPublicationsByTelescopeViewModel>(model);
            ListPublicationsByTelescopeViewModel returnModel = model as ListPublicationsByTelescopeViewModel;
            this.AssertPages(listModel, returnModel);
            this.AssertListPublicationsByTelescopeViewModel(listModel, returnModel);
        }

        [Fact]
        public void ByTelescope_WithNotExistingTelescopeId_ShouldReturnView()
        {
            // Arrange
            Mock<IPublicationService> publicationService = new Mock<IPublicationService>();
            Mock<ITelescopeService> telescopeService = new Mock<ITelescopeService>();

            ListPublicationsByTelescopeViewModel listModel = this.GetListPublicationsByTelescopeViewModel();
            IEnumerable<ListPublicationsServiceModel> publications = null;

            publicationService
                .Setup(p => p.TotalByTelescope(It.IsAny<int>()))
                .Returns(20);

            publicationService
                .Setup(p => p.AllByTelescope(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(publications);

            telescopeService
                .Setup(j => j.GetName(It.IsAny<int>()))
                .Returns("Test Name");

            PublicationsController publicationsController =
                new PublicationsController(null, null, telescopeService.Object, publicationService.Object, null);

            // Act
            IActionResult result = publicationsController.ByTelescope(1, 2);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        private void AssertListPublicationsViewModel(ListPublicationsViewModel expected, ListPublicationsViewModel actual)
        {
            this.AssertPublicationListServiceModel(expected.Publications.First(), actual.Publications.First());
            this.AssertPublicationListServiceModel(expected.Publications.Last(), actual.Publications.Last());
        }

        private void AssertListPublicationsByJournalViewModel(ListPublicationsByJournalViewModel expected, ListPublicationsByJournalViewModel actual)
        {
            Assert.Equal(expected.JournalId, expected.JournalId);
            Assert.Equal(expected.JournalName, expected.JournalName);

            this.AssertPublicationListServiceModel(expected.Publications.First(), actual.Publications.First());
            this.AssertPublicationListServiceModel(expected.Publications.Last(), actual.Publications.Last());
        }

        private void AssertListPublicationsByTelescopeViewModel(ListPublicationsByTelescopeViewModel expected, ListPublicationsByTelescopeViewModel actual)
        {
            Assert.Equal(expected.TelescopeId, expected.TelescopeId);
            Assert.Equal(expected.TelescopeName, expected.TelescopeName);

            this.AssertPublicationListServiceModel(expected.Publications.First(), actual.Publications.First());
            this.AssertPublicationListServiceModel(expected.Publications.Last(), actual.Publications.Last());
        }

        private void AssertPublicationDetailsViewModel(PublicationDetailsViewModel expected, PublicationDetailsViewModel actual)
        {
            this.AssertPublicationDetailsServiceModel(expected.Publication, actual.Publication);

            this.AssertComment(expected.Comments.First(), actual.Comments.First());
            this.AssertComment(expected.Comments.Last(), actual.Comments.Last());
        }

        private void AssertPublicationListServiceModel(ListPublicationsServiceModel expected, ListPublicationsServiceModel actual)
        {
            Assert.Equal(expected.Id, expected.Id);
            Assert.Equal(expected.Title, expected.Title);
            Assert.Equal(expected.Content, expected.Content);
            Assert.Equal(expected.Views, expected.Views);
            Assert.Equal(expected.CommentsCount, expected.CommentsCount);
        }

        private void AssertPublicationDetailsServiceModel(PublicationDetailsServiceModel expected, PublicationDetailsServiceModel actual)
        {
            Assert.Equal(expected.Id, expected.Id);
            Assert.Equal(expected.Content, expected.Content);
            Assert.Equal(expected.JournalId, expected.JournalId);
            Assert.Equal(expected.JournalName, expected.JournalName);
            Assert.Equal(expected.TelescopeId, expected.TelescopeId);
            Assert.Equal(expected.TelescopeName, expected.TelescopeName);
            Assert.Equal(expected.StarSystemName, expected.StarSystemName);
            Assert.Equal(expected.Distance, expected.Distance);
            Assert.Equal(expected.AuthorName, expected.AuthorName);
            this.CompareDates(expected.ReleaseDate, actual.ReleaseDate);
        }

        private void AssertComment(ListCommentsServiceModel expected, ListCommentsServiceModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.IsOwner, actual.IsOwner);
            Assert.Equal(expected.ProfileImage, actual.ProfileImage);
            Assert.Equal(expected.IsOwner, actual.IsOwner);
            this.CompareDatesExact(expected.DateAdded, actual.DateAdded);
        }

        private PublicationDetailsServiceModel GetPublicationDetailsServiceModel()
        {
            return new PublicationDetailsServiceModel
            {
                Id = 1,
                Title = $"Publication Title 1",
                Content = "Publication Content",
                JournalId = 1,
                JournalName = "Journal Name",
                TelescopeId = 1,
                TelescopeName = "Telescope Name",
                AuthorName = "Author Name",
                StarSystemName = "Star System Name",
                Distance = 200000,
                ReleaseDate = DateTime.UtcNow.AddMonths(-1)
            };
        }

        private PublicationDetailsViewModel GetPublicationDetailsViewModel()
        {
            return new PublicationDetailsViewModel
            {
                CurrentPage = 2,
                TotalEntries = 20,
                EntriesPerPage = 10,
                Publication = this.GetPublicationDetailsServiceModel(),
                Comments = this.GetComments()
            };
        }

        private ListPublicationsViewModel GetListPublicationsViewModel()
        {
            return new ListPublicationsViewModel
            {
                CurrentPage = 2,
                TotalEntries = 20,
                EntriesPerPage = 10,
                Publications = this.GetPublications()
            };
        }

        private ListPublicationsByJournalViewModel GetListPublicationsByJournalViewModel()
        {
            return new ListPublicationsByJournalViewModel
            {
                CurrentPage = 2,
                TotalEntries = 20,
                EntriesPerPage = 10,
                JournalId = 1,
                JournalName = "Journal Name",
                Publications = this.GetPublications()
            };
        }

        private ListPublicationsByTelescopeViewModel GetListPublicationsByTelescopeViewModel()
        {
            return new ListPublicationsByTelescopeViewModel
            {
                CurrentPage = 2,
                TotalEntries = 20,
                EntriesPerPage = 10,
                TelescopeId = 1,
                TelescopeName = "Journal Name",
                Publications = this.GetPublications()
            };
        }

        private IEnumerable<ListCommentsServiceModel> GetComments()
        {
            List<ListCommentsServiceModel> comments = new List<ListCommentsServiceModel>();

            for (int i = 1; i <= 20; i++)
            {
                comments.Add(new ListCommentsServiceModel
                {
                    Id = i,
                    Content = $"Comment Content {i}",
                    Username = $"Comment Username {i}",
                    ProfileImage = $"Comment Profile Image {i}",
                    DateAdded = DateTime.UtcNow.AddDays(-i).AddHours(-i).AddMinutes(-i),
                    IsOwner = true
                });
            }

            return comments.OrderBy(c => c.DateAdded).Skip(10).Take(10).ToList();
        }

        private IEnumerable<ListPublicationsServiceModel> GetPublications()
        {
            List<Publication> publications = new List<Publication>();

            for (int i = 1; i <= 20; i++)
            {
                publications.Add(new Publication
                {
                    Id = i,
                    Title = $"Publication Title {i}",
                    Content = $"Publication Content {i}",
                    Views = i * i
                });
            }

            return publications
                .OrderByDescending(p => p.ReleaseDate)
                .Skip(10)
                .Take(10)
                .Select(p => new ListPublicationsServiceModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Views = p.Views,
                    CommentsCount = p.Id * p.Id
                })
                .ToList();
        }
    }
}