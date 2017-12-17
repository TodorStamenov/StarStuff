namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Moderator.Implementations;
    using StarStuff.Services.Moderator.Models.Telescopes;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class TelescopeServiceTest : BaseServiceTest
    {
        [Fact]
        public void Exists_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            string telescopeName = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == 1).Name;

            // Act
            bool result = telescopeService.Exists(telescopeName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_WithNotExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);

            string telescopeName = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == 1).Name;

            // Act
            bool result = telescopeService.Exists(telescopeName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Total_ShouldReturnValidTelescopeCount()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);
            int expected = db.Telescopes.Count();

            // Act
            int actual = telescopeService.Total();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetName_WithExistingId_ShouldReturnValidName()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            string expected = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId).Name;

            // Act
            string actual = telescopeService.GetName(telescopeId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetName_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            const int telescopeId = 1;

            // Act
            string result = telescopeService.GetName(telescopeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldReturnCorrectTelescopeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);

            const int telescopeId = 1;

            Telescope telescope = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            int result = telescopeService.Create(
                telescope.Name,
                telescope.Location,
                telescope.Description,
                telescope.MirrorDiameter,
                telescope.ImageUrl);

            // Assert
            Assert.Equal(telescopeId, result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldAddTelescope()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);

            const int telescopeId = 1;

            Telescope expected = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            telescopeService.Create(
                 expected.Name,
                 expected.Location,
                 expected.Description,
                 expected.MirrorDiameter,
                 expected.ImageUrl);

            Telescope actual = db.Telescopes.Find(telescopeId);

            // Assert
            Assert.True(this.CompareTelescopes(expected, actual));
        }

        [Fact]
        public void Create_WithExistingName_ShouldReturnNegativeTelescopeId()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            Telescope telescope = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == 1);

            // Act
            int result = telescopeService.Create(
                telescope.Name,
                telescope.Location,
                telescope.Description,
                telescope.MirrorDiameter,
                telescope.ImageUrl);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            Telescope telescope = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            bool result = telescopeService.Edit(
                telescopeId,
                "Not Existing",
                telescope.Location,
                telescope.Description,
                telescope.MirrorDiameter,
                telescope.ImageUrl);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldEditTelescope()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            Telescope expected = new Telescope
            {
                Id = telescopeId,
                Name = "Test Name",
                Location = "Test Location",
                Description = "Test Description",
                MirrorDiameter = 55.62,
                ImageUrl = "Test Image Url"
            };

            // Act
            telescopeService.Edit(
                telescopeId,
                expected.Name,
                expected.Location,
                expected.Description,
                expected.MirrorDiameter,
                expected.ImageUrl);

            Telescope actual = db.Telescopes.Find(telescopeId);

            // Assert
            Assert.True(this.CompareTelescopes(expected, actual));
        }

        [Fact]
        public void Edit_WithExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            Telescope telescope = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == 2);

            // Act
            bool result = telescopeService.Edit(
                telescopeId,
                telescope.Name,
                telescope.Location,
                telescope.Description,
                telescope.MirrorDiameter,
                telescope.ImageUrl);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithSameName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            string telescopeName = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId).Name;

            Telescope expected = new Telescope
            {
                Id = telescopeId,
                Name = telescopeName,
                Location = "Test Location",
                Description = "Test Description",
                MirrorDiameter = 55.62,
                ImageUrl = "Test Image Url"
            };

            // Act
            telescopeService.Edit(
                telescopeId,
                expected.Name,
                expected.Location,
                expected.Description,
                expected.MirrorDiameter,
                expected.ImageUrl);

            Telescope actual = db.Telescopes.Find(telescopeId);

            // Assert
            Assert.True(this.CompareTelescopes(expected, actual));
        }

        [Fact]
        public void Edit_WithSameName_ShouldEditTelescope()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            Telescope telescope = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            bool result = telescopeService.Edit(
                telescopeId,
                telescope.Name,
                "Test Location",
                "Test Description",
                56.88,
                "Test Image Url");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);

            const int telescopeId = 1;

            Telescope telescope = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            bool result = telescopeService.Edit(
                telescopeId,
                telescope.Name,
                telescope.Location,
                telescope.Description,
                telescope.MirrorDiameter,
                telescope.ImageUrl);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Details_WithExistingId_ShouldReturnTelescope()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            Telescope expected = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            TelescopeDetailsServiceModel actual = telescopeService.Details(telescopeId);

            // Assert
            Assert.True(this.CompareTelescopes(expected, actual));
        }

        [Fact]
        public void Details_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            const int telescopeId = 1;

            // Act
            TelescopeDetailsServiceModel result = telescopeService.Details(telescopeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetForm_WithExistingId_ShouldReturnTelescope()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int telescopeId = 1;

            Telescope expected = this.GetFakeTelescopes().FirstOrDefault(t => t.Id == telescopeId);

            // Act
            TelescopeFormServiceModel actual = telescopeService.GetForm(telescopeId);

            // Assert
            Assert.True(this.CompareTelescopes(expected, actual));
        }

        [Fact]
        public void GetForm_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            const int telescopeId = 1;

            // Act
            TelescopeFormServiceModel result = telescopeService.GetForm(telescopeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TelescopeDropdown_ShouldReturnValidTelescopes()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            // Act
            IEnumerable<TelescopeServiceModel> telescopes = telescopeService.TelescopeDropdown();
            List<Telescope> fakeTelescopes = this.GetFakeTelescopes().ToList();

            // Assert
            foreach (var actual in telescopes)
            {
                Telescope expected = fakeTelescopes.FirstOrDefault(t => t.Id == actual.Id);

                Assert.True(this.CompareTelescopes(expected, actual));
            }
        }

        [Fact]
        public void All_ShouldReturnValidTelescopes()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            TelescopeService telescopeService = new TelescopeService(db);
            this.SeedDatabase(db);

            const int page = 2;
            const int pageSize = 5;

            // Act
            IEnumerable<ListTelescopesServiceModel> telescopes = telescopeService.All(page, pageSize);
            List<Telescope> fakeTelescopes = this.GetFakeTelescopes()
                .OrderBy(t => t.MirrorDiameter)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int i = -1;

            // Assert
            foreach (var actual in telescopes)
            {
                Telescope expected = fakeTelescopes[++i];

                Assert.True(this.CompareTelescopes(expected, actual));
            }
        }

        private bool CompareTelescopes(Telescope expected, TelescopeServiceModel actual)
        {
            return expected.Id == actual.Id
               && expected.Name == actual.Name;
        }

        private bool CompareTelescopes(Telescope expected, TelescopeFormServiceModel actual)
        {
            return expected.Name == actual.Name
                && expected.Location == actual.Location
                && expected.Description == actual.Description
                && expected.MirrorDiameter == actual.MirrorDiameter
                && expected.ImageUrl == actual.ImageUrl;
        }

        private bool CompareTelescopes(Telescope expected, TelescopeDetailsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Location == actual.Location
                && expected.Description == actual.Description
                && expected.MirrorDiameter == actual.MirrorDiameter
                && expected.ImageUrl == actual.ImageUrl;
        }

        private bool CompareTelescopes(Telescope expected, ListTelescopesServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Description == actual.Description
                && expected.ImageUrl == actual.ImageUrl;
        }

        private bool CompareTelescopes(Telescope expected, Telescope actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Location == actual.Location
                && expected.Description == actual.Description
                && expected.MirrorDiameter == actual.MirrorDiameter
                && expected.ImageUrl == actual.ImageUrl;
        }

        private void SeedDatabase(StarStuffDbContext db)
        {
            db.Telescopes.AddRange(this.GetFakeTelescopes());
            db.SaveChanges();
        }

        private List<Telescope> GetFakeTelescopes()
        {
            List<Telescope> telescopes = new List<Telescope>();

            for (int i = 1; i <= 10; i++)
            {
                telescopes.Add(new Telescope
                {
                    Id = i,
                    Name = $"Telescope Name {i}",
                    Location = $"Telescope Location {i}",
                    Description = $"Telescope Description {i}",
                    MirrorDiameter = i * 2.2,
                    ImageUrl = $"Telescope Image Url"
                });
            }

            return telescopes;
        }
    }
}