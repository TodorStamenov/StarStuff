﻿namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Moderator.Implementations;
    using StarStuff.Services.Moderator.Models.Publications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PublicationServiceTest : BaseServiceTest
    {
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
        public void Create_WithExistingJournalAndDiscovery_ShouldReturnPublicationId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int publicationId = 1;

            Journal journal = new Journal { Id = journalId };
            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                IsConfirmed = true
            };

            db.Journals.Add(journal);
            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Publication publication = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            int actual = publicationService.Create(publication.Content, discoveryId, journalId);

            // Assert
            Assert.Equal(publicationId, actual);
        }

        [Fact]
        public void Create_WithExistingJournalAndDiscovery_ShouldAddPublication()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int discoveryId = 1;
            const int journalId = 1;
            const int publicationId = 1;

            Journal journal = new Journal { Id = journalId };
            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                IsConfirmed = true
            };

            db.Journals.Add(journal);
            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Publication expected = new Publication
            {
                Id = publicationId,
                Content = "Test Content",
                DiscoveryId = discoveryId,
                JournalId = journalId,
                ReleaseDate = DateTime.UtcNow.Date
            };

            // Act
            publicationService.Create(expected.Content, discoveryId, journalId);
            Publication actual = db.Publications.Find(publicationId);

            // Assert
            Assert.True(this.ComparePublications(expected, actual));
        }

        [Fact]
        public void Create_WithNotExistingJournal_ShouldReturnNegativeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int journalId = 1;
            const int discoveryId = 1;
            const int publicationId = 1;

            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                IsConfirmed = true
            };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Publication publication = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            int result = publicationService.Create(publication.Content, discoveryId, journalId);

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
            const int publicationId = 1;

            Journal journal = new Journal { Id = journalId };

            db.Journals.Add(journal);
            db.SaveChanges();

            Publication publication = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            int result = publicationService.Create(publication.Content, discoveryId, journalId);

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
            const int publicationId = 1;

            Journal journal = new Journal { Id = journalId };
            Discovery discovery = new Discovery
            {
                Id = discoveryId
            };

            db.Journals.Add(journal);
            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Publication publication = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            int result = publicationService.Create(publication.Content, discoveryId, journalId);

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

            Journal journal = new Journal { Id = journalId };
            Discovery discovery = new Discovery
            {
                Id = discoveryId
            };

            Publication publication = new Publication
            {
                Content = "Test Content",
                DiscoveryId = discoveryId,
                JournalId = journalId
            };

            db.Journals.Add(journal);
            db.Discoveries.Add(discovery);
            db.Publications.Add(publication);
            db.SaveChanges();

            // Act
            int result = publicationService.Create(publication.Content, discoveryId, journalId);

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

            Publication publication = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            bool result = publicationService.Edit(publicationId, publication.Content);

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
                Content = "Test Content",
                ReleaseDate = this.GetFakePublications()
                    .FirstOrDefault(p => p.Id == publicationId)
                    .ReleaseDate
            };

            // Act
            publicationService.Edit(publicationId, expected.Content);
            Publication actual = db.Publications.Find(publicationId);

            // Assert
            Assert.True(this.ComparePublications(expected, actual));
        }

        [Fact]
        public void Edit_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PublicationService publicationService = new PublicationService(db);

            const int publicationId = 1;

            Publication publication = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            bool result = publicationService.Edit(publicationId, publication.Content);

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

            Publication expected = this.GetFakePublications().FirstOrDefault(p => p.Id == publicationId);

            // Act
            PublicationFormServiceModel actual = publicationService.GetForm(publicationId);

            // Assert
            Assert.True(this.ComparePublications(expected, actual));
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

            Journal journal = new Journal { Id = journalId };

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
            }

            db.Journals.Add(journal);
            db.Telescopes.Add(telescope);
            db.Publications.AddRange(publications);
            db.SaveChanges();

            Publication expected = publications.FirstOrDefault(p => p.Id == publicationId);

            // Act
            PublicationDetailsServiceModel actual = publicationService.Details(publicationId);

            // Assert
            Assert.True(this.ComparePublications(expected, actual));
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

            Journal journal = new Journal { Id = journalId };
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

            db.Journals.Add(journal);
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

            // Act
            IEnumerable<ListPublicationsServiceModel> publications = publicationService.All(page, pageSize);

            List<Publication> fakePublications = this.GetFakePublications()
                .OrderByDescending(p => p.ReleaseDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Assert
            foreach (var actual in publications)
            {
                Publication expected = fakePublications[++i];

                Assert.True(this.ComparePublications(expected, actual));
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

            // Act
            IEnumerable<ListPublicationsServiceModel> publications = publicationService.AllByJournal(journalId, page, pageSize);

            List<Publication> fakePublications = this.GetFakePublications()
                .OrderByDescending(p => p.ReleaseDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Assert
            foreach (var actual in publications)
            {
                Publication expected = fakePublications[++i];

                Assert.True(this.ComparePublications(expected, actual));
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

            // Act
            IEnumerable<ListPublicationsServiceModel> publications = publicationService.AllByTelescope(telescopeId, page, pageSize);

            List<Publication> fakePublications = this.GetFakePublications()
                .OrderByDescending(p => p.ReleaseDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Assert
            foreach (var actual in publications)
            {
                Publication expected = fakePublications[++i];

                Assert.True(this.ComparePublications(expected, actual));
            }
        }

        private bool ComparePublications(Publication expected, Publication actual)
        {
            return expected.Id == actual.Id
                && expected.Content == actual.Content
                && expected.ReleaseDate == actual.ReleaseDate;
        }

        private bool ComparePublications(Publication expected, PublicationDetailsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Content == actual.Content
                && expected.ReleaseDate == actual.ReleaseDate
                && expected.JournalId == actual.JournalId
                && expected.Discovery.Telescope.Name == actual.TelescopeName
                && expected.Discovery.StarSystem == actual.StarSystemName
                && expected.Discovery.TelescopeId == actual.TelescopeId;
        }

        private bool ComparePublications(Publication expected, PublicationFormServiceModel actual)
        {
            return expected.Content == actual.Content;
        }

        private bool ComparePublications(Publication expected, ListPublicationsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Content == actual.Content;
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
                    Content = $"Publication Content {i}",
                    ReleaseDate = DateTime.UtcNow.Date.AddDays(-i)
                });
            }

            return publications;
        }
    }
}