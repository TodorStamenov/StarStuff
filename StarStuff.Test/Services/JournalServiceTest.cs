namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Moderator.Implementations;
    using StarStuff.Services.Moderator.Models.Journals;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class JournalServiceTest : BaseServiceTest
    {
        [Fact]
        public void Exists_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            // Act
            string name = this.GetFakeJournals().FirstOrDefault(s => s.Id == 1).Name;
            bool result = journalService.Exists(name);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_WithNotExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);

            // Act
            string name = this.GetFakeJournals().FirstOrDefault(s => s.Id == 1).Name;
            bool result = journalService.Exists(name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Total_ShouldReturnJournalsCount()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            int expected = this.GetFakeJournals().Count;

            // Act
            int actual = journalService.Total();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetName_WithExistingId_ShouldReturnJournalsName()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;
            string expected = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId).Name;

            // Act
            string actual = journalService.GetName(journalId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetName_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);

            // Act
            string result = journalService.GetName(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldReturnJournalId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);

            const int journalId = 1;

            Journal journal = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            // Act
            int result = journalService.Create(journal.Name, journal.Description, journal.ImageUrl);

            // Assert
            Assert.Equal(journalId, result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldAddJournal()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);

            const int journalId = 1;

            Journal expected = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            // Act
            journalService.Create(expected.Name, expected.Description, expected.ImageUrl);
            Journal actual = db.Journals.Find(journalId);

            // Assert
            Assert.True(this.CompareJournals(expected, actual));
        }

        [Fact]
        public void Create_WithExistingName_ShouldReturnNegativeInt()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal journal = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            // Act
            int result = journalService.Create(journal.Name, journal.Description, journal.ImageUrl);

            // Assert
            Assert.True(result <= 0);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal journal = new Journal
            {
                Name = "Not Existing",
                Description = "Not Existing",
                ImageUrl = "Not Existing"
            };

            // Act
            bool result = journalService.Edit(journalId, journal.Name, journal.Description, journal.ImageUrl);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldEditJournal()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal expected = new Journal
            {
                Id = journalId,
                Name = "Not Existing",
                Description = "Not Existing",
                ImageUrl = "Not Existing"
            };

            // Act
            journalService.Edit(journalId, expected.Name, expected.Description, expected.ImageUrl);
            Journal actual = db.Journals.Find(journalId);

            // Assert
            Assert.True(this.CompareJournals(expected, actual));
        }

        [Fact]
        public void Edit_WithSameName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal expected = new Journal
            {
                Id = journalId,
                Name = "Not Existing",
                Description = "Not Existing",
                ImageUrl = "Not Existing"
            };

            // Act
            bool result = journalService.Edit(journalId, expected.Name, expected.Description, expected.ImageUrl);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithSameName_ShouldEditJournal()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal journal = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            Journal expected = new Journal
            {
                Id = journalId,
                Name = journal.Name,
                Description = "Not Existing",
                ImageUrl = "Not Existing"
            };

            // Act
            journalService.Edit(journalId, expected.Name, expected.Description, expected.ImageUrl);
            Journal actual = db.Journals.Find(journalId);

            // Assert
            Assert.True(this.CompareJournals(expected, actual));
        }

        [Fact]
        public void Edit_WithExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal journal = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            // Act
            bool result = journalService.Edit(2, journal.Name, journal.Description, journal.ImageUrl);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Details_WithExistingId_ShouldReturnDetails()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal expected = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            // Act
            JournalDetailsServiceModel actual = journalService.Details(journalId);

            // Assert
            Assert.True(this.CompareJournals(expected, actual));
        }

        [Fact]
        public void Details_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);

            const int journalId = 1;

            // Act
            JournalDetailsServiceModel actual = journalService.Details(journalId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetForm_WithExistingId_ShouldReturnForm()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int journalId = 1;

            Journal expected = this.GetFakeJournals().FirstOrDefault(j => j.Id == journalId);

            // Act
            JournalFormServiceModel actual = journalService.GetForm(journalId);

            // Assert
            Assert.True(this.CompareJournals(expected, actual));
        }

        [Fact]
        public void GetForm_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);

            const int journalId = 1;

            // Act
            JournalFormServiceModel actual = journalService.GetForm(journalId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void All_ShouldReturnValidJournals()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            JournalService journalService = new JournalService(db);
            this.SeedDatabase(db);

            const int page = 2;
            const int pageSize = 5;

            // Act
            IEnumerable<ListJournalsServiceModel> journals = journalService.All(page, pageSize);
            List<Journal> fakeJournals = this.GetFakeJournals()
                .OrderBy(j => j.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Assert
            foreach (var actual in journals)
            {
                Journal expected = fakeJournals[++i];

                Assert.True(this.CompareJournals(expected, actual));
            }
        }

        private bool CompareJournals(Journal expected, Journal actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Description == actual.Description
                && expected.ImageUrl == actual.ImageUrl;
        }

        private bool CompareJournals(Journal expected, JournalDetailsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Description == actual.Description
                && expected.ImageUrl == actual.ImageUrl;
        }

        private bool CompareJournals(Journal expected, JournalFormServiceModel actual)
        {
            return expected.Name == actual.Name
                && expected.Description == actual.Description
                && expected.ImageUrl == actual.ImageUrl;
        }

        private bool CompareJournals(Journal expected, ListJournalsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Description == actual.Description
                && expected.ImageUrl == actual.ImageUrl;
        }

        private void SeedDatabase(StarStuffDbContext db)
        {
            db.Journals.AddRange(this.GetFakeJournals());
            db.SaveChanges();
        }

        private List<Journal> GetFakeJournals()
        {
            List<Journal> journals = new List<Journal>();

            for (int i = 1; i <= 10; i++)
            {
                journals.Add(new Journal
                {
                    Id = i,
                    Name = $"Journal Name {i}",
                    Description = $"Journal Description {i}",
                    ImageUrl = $"Journal Image {i}"
                });
            }

            return journals;
        }
    }
}