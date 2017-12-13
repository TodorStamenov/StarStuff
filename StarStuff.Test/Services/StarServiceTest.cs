namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Astronomer.Implementations;
    using StarStuff.Services.Astronomer.Models.Stars;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class StarServiceTest : BaseTest
    {
        [Fact]
        public void Exists_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            // Act
            string name = this.GetFakeStars().FirstOrDefault(s => s.Id == 1).Name;
            bool result = starService.Exists(name);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_WithNotExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            string name = this.GetFakeStars().FirstOrDefault(s => s.Id == 1).Name;

            // Act
            bool result = starService.Exists(name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int discoveryId = 1;
            Discovery discovery = new Discovery { Id = discoveryId };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            // Act
            bool result = starService.Create(discoveryId, "Not Existing Name", 200000);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldAddStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int discoveryId = 1;
            const int starId = 1;

            Discovery discovery = new Discovery { Id = discoveryId };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Star expected = this.GetFakeStars().FirstOrDefault(s => s.Id == starId);

            // Act
            starService.Create(discoveryId, expected.Name, expected.Temperature);

            Star actual = db.Stars.Find(starId);

            // Assert
            Assert.True(this.CompareStars(expected, actual));
        }

        [Fact]
        public void Create_WithNotExistingDeiscovery_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int discoveryId = 1;
            const int starId = 1;

            Star expected = this.GetFakeStars().FirstOrDefault(s => s.Id == starId);

            // Act
            bool result = starService.Create(discoveryId, expected.Name, expected.Temperature);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            const int starId = 1;

            Discovery discovery = new Discovery { Id = discoveryId };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Star star = this.GetFakeStars().FirstOrDefault(s => s.Id == starId);

            // Act
            bool result = starService.Create(discoveryId, star.Name, star.Temperature);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithDiscoveryWithMaxStars_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int discoveryId = 1;

            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                Stars = this.GetFakeStars().OrderBy(s => s.Id).Take(3).ToList()
            };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Star star = this.GetFakeStars().FirstOrDefault(s => s.Id == 4);

            // Act
            bool result = starService.Create(discoveryId, star.Name, star.Temperature);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            Star star = this.GetFakeStars().FirstOrDefault(s => s.Id == starId);

            // Act
            bool result = starService.Edit(star.Id, "Not Existing Name", 400000);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldEditStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            Star expected = new Star
            {
                Id = starId,
                Name = "Not Existing Name",
                Temperature = 400000
            };

            // Act
            starService.Edit(starId, expected.Name, expected.Temperature);
            Star actual = db.Stars.Find(starId);

            // Assert
            Assert.True(this.CompareStars(expected, actual));
        }

        [Fact]
        public void Edit_WithSameName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            Star star = this.GetFakeStars().FirstOrDefault(s => s.Id == 1);

            // Act
            bool result = starService.Edit(starId, star.Name, 500000);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithSameName_ShouldEditStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            string name = GetFakeStars().FirstOrDefault(s => s.Id == starId).Name;
            Star expected = new Star
            {
                Id = starId,
                Name = name,
                Temperature = 500000
            };

            // Act
            starService.Edit(starId, expected.Name, expected.Temperature);
            Star actual = db.Stars.Find(starId);

            // Assert
            Assert.True(this.CompareStars(expected, actual));
        }

        [Fact]
        public void Edit_WithNotValidId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int starId = 1;

            Star star = new Star
            {
                Name = "Not Existing Name",
                Temperature = 400000
            };

            // Act
            bool result = starService.Edit(starId, star.Name, star.Temperature);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            Star star = this.GetFakeStars().FirstOrDefault(s => s.Id == 2);

            // Act
            bool result = starService.Edit(starId, star.Name, star.Temperature);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            // Act
            bool result = starService.Delete(starId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_WithValidId_ShouldRemoveStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            // Act
            starService.Delete(starId);

            // Assert
            Assert.Equal(9, db.Stars.Count());
        }

        [Fact]
        public void Delete_WithNotValidId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int starId = 1;

            // Act
            bool result = starService.Delete(starId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetName_WithValidId_ShouldReturnValidName()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            Star star = this.GetFakeStars().FirstOrDefault(s => s.Id == 1);

            // Act
            string actual = starService.GetName(star.Id);

            // Assert
            Assert.Equal(star.Name, actual);
        }

        [Fact]
        public void GetName_WithNotValidId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            // Act
            string result = starService.GetName(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetForm_WithValidId_ShouldReturnStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            Star expected = this.GetFakeStars().FirstOrDefault(s => s.Id == starId);

            // Act
            StarFormServiceModel actual = starService.GetForm(starId);

            // Assert
            Assert.True(this.CompareStars(expected, actual));
        }

        [Fact]
        public void GetForm_WithNotValidId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            // Act
            StarFormServiceModel result = starService.GetForm(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Stars_WithValidDiscoveryId_ShouldReturnStars()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int discoveryId = 1;

            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                Stars = this.GetFakeStars()
            };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            // Act
            IEnumerable<ListStarsServiceModel> stars = starService.Stars(discoveryId);

            // Assert
            foreach (var expected in this.GetFakeStars())
            {
                ListStarsServiceModel actual = stars.FirstOrDefault(s => s.Id == expected.Id);

                Assert.True(this.CompareStars(expected, actual));
            }
        }

        [Fact]
        public void Stars_WithNotValidDiscoveryId_ShouldReturnEmptyCollection()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int discoveryId = 1;

            // Act
            IEnumerable<ListStarsServiceModel> stars = starService.Stars(discoveryId);

            // Assert
            Assert.False(stars.Any());
        }

        private bool CompareStars(Star expected, Star actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Temperature == actual.Temperature;
        }

        private bool CompareStars(Star expected, StarFormServiceModel actual)
        {
            return expected.Name == actual.Name
                && expected.Temperature == actual.Temperature;
        }

        private bool CompareStars(Star expected, ListStarsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Temperature == actual.Temperature;
        }

        private void SeedDatabase(StarStuffDbContext db)
        {
            db.Stars.AddRange(this.GetFakeStars());
            db.SaveChanges();
        }

        private List<Star> GetFakeStars()
        {
            List<Star> stars = new List<Star>();

            for (int i = 1; i <= 10; i++)
            {
                stars.Add(new Star
                {
                    Id = i,
                    Name = $"Star Name {i}",
                    Temperature = 300000
                });
            }

            return stars;
        }
    }
}