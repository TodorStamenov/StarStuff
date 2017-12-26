namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Areas.Moderator.Implementations;
    using StarStuff.Services.Areas.Moderator.Models.Publications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PublicationServiceTest : BaseServiceTest
    {
        [Fact]
        public void Exists_WithExistingJournalAndDiscovery_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            const int journalId = 1;

            Publication publication = db.Publications.Find(1);
            publication.DiscoveryId = discoveryId;
            publication.JournalId = journalId;
            db.SaveChanges();

            // Act
            bool result = publicationService.Exists(journalId, discoveryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_WithNotExistingJournal_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            const int journalId = 1;

            Publication publication = db.Publications.Find(1);
            publication.DiscoveryId = discoveryId;
            db.SaveChanges();

            // Act
            bool result = publicationService.Exists(journalId, discoveryId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Exists_WithNotExistingDiscovery_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            const int journalId = 1;

            Publication publication = db.Publications.Find(1);
            publication.JournalId = journalId;
            db.SaveChanges();

            // Act
            bool result = publicationService.Exists(journalId, discoveryId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Exists_WithNotExistingJournalAndDiscovery_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            const int journalId = 1;

            // Act
            bool result = publicationService.Exists(journalId, discoveryId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TitleExists_WithExistingPublicationTitle_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            string title = this.GetFakePublications().First().Title;

            // Act
            bool result = publicationService.TitleExists(title);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TitleExists_WithNotExistingPublicationTitle_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            string title = this.GetFakePublications().First().Title;

            // Act
            bool result = publicationService.TitleExists(title);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetTitle_WithExistingPublicationId_ShouldReturnPublicationTitle()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            string expected = this.GetFakePublications().First().Title;

            // Act
            string actual = publicationService.GetTitle(1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTitle_WithNotExistingPublicationId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            // Act
            string result = publicationService.GetTitle(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Total_ShouldReturnPublicationsCount()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            int expected = this.GetFakePublications().Count;

            // Act
            int actual = publicationService.Total();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TotalByJournal_ShouldReturnPublicationsCount()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int journalId = 1;
            const int publicationsCount = 3;

            Journal journal = new Journal
            {
                Id = journalId,
                Publications = this.GetFakePublications()
                    .OrderBy(p => p.Id)
                    .Take(publicationsCount)
                    .ToList()
            };

            db.Journals.Add(journal);
            db.SaveChanges();

            // Act
            int actual = publicationService.TotalByJournal(journalId);

            // Assert
            Assert.Equal(publicationsCount, actual);
        }

        [Fact]
        public void TotalByTelescope_ShouldReturnPublicationsCount()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int telescopeId = 1;
            const int publicationsCount = 3;

            Telescope telescopes = new Telescope
            {
                Id = telescopeId,
                Discoveries = new List<Discovery>()
                {
                    new Discovery
                    {
                        Publications = this.GetFakePublications()
                            .OrderBy(p => p.Id)
                            .Take(publicationsCount)
                            .ToList()
                    }
                }
            };

            db.Telescopes.Add(telescopes);
            db.SaveChanges();

            // Act
            int actual = publicationService.TotalByTelescope(telescopeId);

            // Assert
            Assert.Equal(publicationsCount, actual);
        }

        [Fact]
        public void Create_WithExistingJournalUserDiscoveryAndNotExistingTitle_ShouldReturnPublicationId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int publicationId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedUser(db);
            this.SeedDiscovery(db, true);

            Publication publication = this.GetFakePublications().First();

            // Act
            int actual = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.Equal(publicationId, actual);
        }

        [Fact]
        public void Create_WithExistingUserJournalDiscoveryAndNotExistingTitle_ShouldAddPublication()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int publicationId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedUser(db);
            this.SeedDiscovery(db, true);

            Publication expected = this.GetFakePublications().First();
            expected.Views = 0;
            expected.ReleaseDate = DateTime.UtcNow;

            // Act
            publicationService.Create(expected.Title, expected.Content, discoveryId, journalId, authorId);
            Publication actual = db.Publications.Find(publicationId);

            // Assert
            this.ComparePublications(expected, actual);
        }

        [Fact]
        public void Create_WithNotExistingJournal_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int journalId = 1;
            const int discoveryId = 1;
            const int authorId = 1;

            this.SeedUser(db);
            this.SeedDiscovery(db, true);

            Publication publication = this.GetFakePublications().First();

            // Act
            int result = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Create_WithNotExistingDiscovery_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int authorId = 1;

            this.SeedUser(db);
            this.SeedJournal(db);

            Publication publication = this.GetFakePublications().First();

            // Act
            int result = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Create_WithNotExistingUser_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedDiscovery(db, true);

            Publication publication = this.GetFakePublications().First();

            // Act
            int result = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Create_WithNotConfirmedDiscovery_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedUser(db);
            this.SeedDiscovery(db, false);

            Publication publication = this.GetFakePublications().First();

            // Act
            int result = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Create_WithExistingTitle_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedUser(db);
            this.SeedDiscovery(db, false);

            Publication publication = this.GetFakePublications().First();

            // Act
            int result = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Create_WithRepeatingPublication_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedUser(db);
            this.SeedDiscovery(db, true);

            Publication publication = GetFakePublications().First();
            publication.DiscoveryId = discoveryId;
            publication.JournalId = journalId;

            db.Publications.Add(publication);
            db.SaveChanges();

            // Act
            int result = publicationService.Create(publication.Title, publication.Content, discoveryId, journalId, authorId);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Edit_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int publicationId = 1;

            Publication publication = this.GetFakePublications().First();

            // Act
            bool result = publicationService.Edit(publicationId, publication.Title, publication.Content);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithExistingId_ShouldEditPublcation()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int publicationId = 1;

            Publication expected = new Publication
            {
                Id = publicationId,
                Title = "Fake Title",
                Content = "Fake Content",
                Views = this.GetFakePublications().First().Views,
                ReleaseDate = this.GetFakePublications().First().ReleaseDate
            };

            // Act
            publicationService.Edit(publicationId, expected.Title, expected.Content);
            Publication actual = db.Publications.Find(publicationId);

            // Assert
            this.ComparePublications(expected, actual);
        }

        [Fact]
        public void Edit_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int publicationId = 1;

            Publication publication = this.GetFakePublications().First();

            // Act
            bool result = publicationService.Edit(publicationId, publication.Title, publication.Content);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetForm_WithExistingId_ShouldReturnPublication()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int publicationId = 1;

            Publication expected = this.GetFakePublications().First();

            // Act
            PublicationFormServiceModel actual = publicationService.GetForm(publicationId);

            // Assert
            this.ComparePublications(expected, actual);
        }

        [Fact]
        public void GetForm_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int publicationId = 1;

            // Act
            PublicationFormServiceModel result = publicationService.GetForm(publicationId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Details_WithExistingId_ShouldReturnValidPublicationDetails()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int publicationId = 1;
            const int journalId = 1;
            const int discoveryId = 1;
            const int telescopeId = 1;
            const int authorId = 1;

            this.SeedJournal(db);
            this.SeedUser(db);

            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                StarSystem = "Star System Name"
            };

            Telescope telescope = new Telescope
            {
                Id = telescopeId,
                Name = "Telescope Name",
                Discoveries = new List<Discovery>
                {
                    discovery
                }
            };

            List<Publication> publications = this.GetFakePublications();

            foreach (var publication in publications)
            {
                publication.DiscoveryId = discoveryId;
                publication.JournalId = journalId;
                publication.AuthorId = authorId;
            }

            db.Telescopes.Add(telescope);
            db.Publications.AddRange(publications);
            db.SaveChanges();

            Publication expected = publications.First();

            // Act
            PublicationDetailsServiceModel actual = publicationService.Details(publicationId);

            // Assert
            this.ComparePublications(expected, actual);
        }

        [Fact]
        public void Details_ShouldIncreaseViews()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int publicationId = 1;

            int expected = 2;

            // Act
            publicationService.Details(publicationId);

            Publication publication = db.Publications.Find(publicationId);

            // Assert
            Assert.Equal(expected, publication.Views);
        }

        [Fact]
        public void Details_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int journalId = 1;
            const int discoveryId = 1;
            const int telescopeId = 1;

            this.SeedJournal(db);
            Discovery discovery = new Discovery { Id = discoveryId };

            Telescope telescope = new Telescope
            {
                Id = telescopeId,
                Discoveries = new List<Discovery>
                {
                    discovery
                }
            };

            List<Publication> publications = this.GetFakePublications();

            foreach (var publication in publications)
            {
                publication.DiscoveryId = discoveryId;
                publication.JournalId = journalId;
            }

            db.Telescopes.Add(telescope);
            db.Publications.AddRange(publications);
            db.SaveChanges();

            // Act
            PublicationDetailsServiceModel result = publicationService.Details(11);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void All_ShouldReturnValidPublications()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);
            this.SeedDatabase(db);

            const int page = 2;
            const int pageSize = 5;

            List<Publication> fakePublications = this.GetFakePublications()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Act
            IEnumerable<ListPublicationsServiceModel> publications = publicationService.All(page, pageSize);

            // Assert
            foreach (var actual in publications)
            {
                Publication expected = fakePublications[++i];

                this.ComparePublications(expected, actual);
            }
        }

        [Fact]
        public void AllByJournal_ShouldReturnValidPublications()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int page = 2;
            const int pageSize = 5;
            const int journalId = 1;

            Journal journal = new Journal
            {
                Id = journalId,
                Publications = GetFakePublications()
                    .OrderBy(p => p.Id)
                    .ToList()
            };

            db.Journals.Add(journal);
            db.SaveChanges();

            List<Publication> fakePublications = this.GetFakePublications()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Act
            IEnumerable<ListPublicationsServiceModel> publications = publicationService
                .AllByJournal(journalId, page, pageSize);

            // Assert
            foreach (var actual in publications)
            {
                Publication expected = fakePublications[++i];

                this.ComparePublications(expected, actual);
            }
        }

        [Fact]
        public void AllByTelescope_ShouldReturnValidPublications()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int page = 2;
            const int pageSize = 5;
            const int telescopeId = 1;

            Telescope telescope = new Telescope
            {
                Id = telescopeId,
                Discoveries = new List<Discovery>
                {
                    new Discovery
                    {
                        Publications = this.GetFakePublications()
                    }
                }
            };

            db.Telescopes.Add(telescope);
            db.SaveChanges();

            List<Publication> fakePublications = this.GetFakePublications()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Act
            IEnumerable<ListPublicationsServiceModel> publications = publicationService
                .AllByTelescope(telescopeId, page, pageSize);

            // Assert
            foreach (var actual in publications)
            {
                Publication expected = fakePublications[++i];

                this.ComparePublications(expected, actual);
            }
        }

        private void ComparePublications(Publication expected, Publication actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Content, actual.Content);
            this.CompareDates(expected.ReleaseDate, actual.ReleaseDate);
        }

        private void ComparePublications(Publication expected, PublicationDetailsServiceModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.JournalId, actual.JournalId);
            Assert.Equal(expected.Discovery.Telescope.Name, actual.TelescopeName);
            Assert.Equal(expected.Discovery.StarSystem, actual.StarSystemName);
            Assert.Equal(expected.Discovery.TelescopeId, actual.TelescopeId);
            this.CompareDates(expected.ReleaseDate, actual.ReleaseDate);
        }

        private void ComparePublications(Publication expected, PublicationFormServiceModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Content, actual.Content);
        }

        private void ComparePublications(Publication expected, ListPublicationsServiceModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.Views, actual.Views);
        }

        private void SeedDiscovery(StarStuffDbContext db, bool confirmed)
        {
            db.Discoveries.Add(new Discovery
            {
                Id = 1,
                IsConfirmed = confirmed
            });

            db.SaveChanges();
        }

        private void SeedJournal(StarStuffDbContext db)
        {
            db.Journals.Add(new Journal { Id = 1 });
            db.SaveChanges();
        }

        private void SeedUser(StarStuffDbContext db)
        {
            db.Users.Add(new User { Id = 1 });
            db.SaveChanges();
        }

        private void SeedDatabase(StarStuffDbContext db)
        {
            db.Publications.AddRange(this.GetFakePublications());
            db.SaveChanges();
        }

        private List<Publication> GetFakePublications()
        {
            List<Publication> publications = new List<Publication>();

            for (int i = 1; i <= 10; i++)
            {
                publications.Add(new Publication
                {
                    Id = i,
                    Title = $"Publication Title {i}",
                    Content = $"Publication Content {i}",
                    Views = i * i,
                    ReleaseDate = DateTime.UtcNow.Date.AddDays(-i)
                });
            }

            return publications.OrderByDescending(p => p.ReleaseDate).ToList();
        }
    }
}