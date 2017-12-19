namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Astronomer.Implementations;
    using StarStuff.Services.Astronomer.Models.Stars;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class StarServiceTest : BaseServiceTest
    {
        [Fact]
        public void Exists_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            // Act
            string name = this.GetFakeStars().First().Name;
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

            string name = this.GetFakeStars().First().Name;

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
            this.SeedDiscovery(db);

            Star star = this.GetFakeStars().First();

            // Act
            bool result = starService.Create(discoveryId, star.Name, star.Temperature);

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

            this.SeedDiscovery(db);

            Star expected = this.GetFakeStars().First();

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

            Star expected = this.GetFakeStars().First();

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

            this.SeedDiscovery(db);

            Star star = this.GetFakeStars().First();

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

            this.SeedDiscovery(db, 3);

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

            Star star = this.GetFakeStars().First();
            star.Name = "Not Existing Name";
            star.Temperature = 111111;

            // Act
            bool result = starService.Edit(star.Id, star.Name, star.Temperature);

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

            Star expected = this.GetFakeStars().First();
            expected.Name = "Not Existing Name";
            expected.Temperature = 111111;

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

            Star star = this.GetFakeStars().First();
            star.Temperature = 111111;

            // Act
            bool result = starService.Edit(star.Id, star.Name, star.Temperature);

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

            Star expected = this.GetFakeStars().First();
            expected.Temperature = 111111;

            // Act
            starService.Edit(expected.Id, expected.Name, expected.Temperature);
            Star actual = db.Stars.Find(expected.Id);

            // Assert
            Assert.True(this.CompareStars(expected, actual));
        }

        [Fact]
        public void Edit_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            const int starId = 1;

            Star star = this.GetFakeStars().First();

            // Act
            bool result = starService.Edit(starId, star.Name, star.Temperature);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithExistingNewName_ShouldReturnFalse()
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
        public void Delete_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDiscovery(db, 2);

            const int starId = 1;

            // Act
            bool result = starService.Delete(starId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_WithExistingId_ShouldRemoveStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            int starsCount = this.GetFakeStars().Count;
            int starId = 1;

            this.SeedDiscovery(db, starsCount);

            // Act
            starService.Delete(starId);
            Star star = db.Stars.Find(starId);

            // Assert
            Assert.Null(star);
            Assert.Equal(starsCount - 1, db.Stars.Count());
        }

        [Fact]
        public void Delete_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            int starsCount = this.GetFakeStars().Count;
            this.SeedDiscovery(db, starsCount);

            // Act
            bool result = starService.Delete(starsCount + 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_WithNotExistingId_ShouldNotRemoveStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);

            int starsCount = this.GetFakeStars().Count;
            this.SeedDiscovery(db, starsCount);

            // Act
            starService.Delete(11);

            // Assert
            Assert.Equal(starsCount, db.Stars.Count());
        }

        [Fact]
        public void Delete_LastStar_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDiscovery(db, 1);

            // Act
            bool result = starService.Delete(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetName_WithExistingId_ShouldReturnValidName()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            Star star = this.GetFakeStars().First();

            // Act
            string actual = starService.GetName(star.Id);

            // Assert
            Assert.Equal(star.Name, actual);
        }

        [Fact]
        public void GetName_WithNotExistingId_ShouldReturnNull()
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
        public void GetForm_WithExistingId_ShouldReturnStar()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            StarService starService = new StarService(db);
            this.SeedDatabase(db);

            const int starId = 1;

            Star expected = this.GetFakeStars().First();

            // Act
            StarFormServiceModel actual = starService.GetForm(starId);

            // Assert
            Assert.True(this.CompareStars(expected, actual));
        }

        [Fact]
        public void GetForm_WithNotExistingId_ShouldReturnNull()
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
        public void Stars_WithExistingDiscoveryId_ShouldReturnStars()
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
            IEnumerable<ListStarsServiceModel> stars = starService.Stars(discoveryId).OrderBy(s => s.Id);

            // Assert
            foreach (var expected in this.GetFakeStars())
            {
                ListStarsServiceModel actual = stars.FirstOrDefault(s => s.Id == expected.Id);

                Assert.True(this.CompareStars(expected, actual));
            }
        }

        [Fact]
        public void Stars_WithNotExistingDiscoveryId_ShouldReturnEmptyCollection()
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

        private void SeedDiscovery(StarStuffDbContext db, int starsCount = 0)
        {
            db.Discoveries.Add(new Discovery
            {
                Id = 1,
                Stars = this.GetFakeStars().Take(starsCount).ToList()
            });

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
                    Temperature = i * 100000
                });
            }

            return stars.OrderBy(s => s.Id).ToList();
        }
    }
}