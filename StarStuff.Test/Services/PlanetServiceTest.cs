namespace StarStuff.Test.Services
{
    using Data;
    using Data.Models;
    using StarStuff.Services.Astronomer.Implementations;
    using StarStuff.Services.Astronomer.Models.Planets;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PlanetServiceTest : BaseServiceTest
    {
        [Fact]
        public void Exists_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            string name = this.GetFakePlanets().FirstOrDefault(p => p.Id == 1).Name;
            bool result = planetService.Exists(name);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_WithNotExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            string name = this.GetFakePlanets().FirstOrDefault(p => p.Id == 1).Name;

            // Act
            bool result = planetService.Exists(name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            const int discoveryId = 1;
            Discovery discovery = new Discovery { Id = discoveryId };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            // Act
            bool result = planetService.Create(discoveryId, "Not Existing 1", 20.5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Create_WithNotExistingName_ShouldAddPlanet()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            const int discoveryId = 1;
            Discovery discovery = new Discovery
            {
                Id = discoveryId
            };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Planet expected = new Planet
            {
                Id = 1,
                Name = "Not Existing 1",
                Mass = 22.5
            };

            // Act
            planetService.Create(discoveryId, expected.Name, expected.Mass);
            Planet actual = db.Planets.First();

            // Assert
            Assert.Equal(1, db.Planets.Count());
            Assert.True(this.ComparePlanets(expected, actual));
        }

        [Fact]
        public void Create_WithExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            Discovery discovery = new Discovery { Id = discoveryId };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            Planet planet = this.GetFakePlanets().FirstOrDefault(p => p.Id == 1);

            // Act
            bool result = planetService.Create(discoveryId, planet.Name, planet.Mass);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithNotExistingDicovery_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            // Act
            bool result = planetService.Create(1, "Not Existing 1", 20.5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            bool result = planetService.Edit(1, "Not Existing 1", 20.5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithNotExistingName_ShouldEditPlanet()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);
            const int planetId = 1;

            Planet expected = new Planet
            {
                Id = planetId,
                Name = "Not Existing 1",
                Mass = 20.5
            };

            // Act
            bool result = planetService.Edit(planetId, expected.Name, expected.Mass);
            Planet actual = db.Planets.FirstOrDefault(p => p.Id == planetId);

            // Assert
            Assert.True(this.ComparePlanets(expected, actual));
        }

        [Fact]
        public void Edit_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            bool result = planetService.Edit(11, "Not Existing 1", 20.5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithSameName_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            Planet planet = this.GetFakePlanets().FirstOrDefault(p => p.Id == 1);

            // Act
            bool result = planetService.Edit(planet.Id, planet.Name, 20.5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithSameName_ShouldEditPlanet()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            const int planetId = 1;
            string name = this.GetFakePlanets().FirstOrDefault(p => p.Id == planetId).Name;

            Planet expected = new Planet
            {
                Id = planetId,
                Name = name,
                Mass = 20.5
            };

            // Act
            planetService.Edit(expected.Id, expected.Name, expected.Mass);
            Planet actual = db.Planets.Find(planetId);

            // Assert
            Assert.True(this.ComparePlanets(expected, actual));
        }

        [Fact]
        public void Edit_WithDifferentExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            Planet planet = this.GetFakePlanets().FirstOrDefault(p => p.Id == 2);

            // Act
            bool result = planetService.Edit(1, planet.Name, planet.Mass);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            bool result = planetService.Delete(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_WithExistingId_ShouldRemovePlanet()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            planetService.Delete(1);
            Planet planet = db.Planets.Find(1);

            // Assert
            Assert.Equal(9, db.Planets.Count());
            Assert.Null(planet);
        }

        [Fact]
        public void Delete_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            bool result = planetService.Delete(11);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetName_WithExistingId_ShouldReturnValidName()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            Planet planet = this.GetFakePlanets().FirstOrDefault(p => p.Id == 1);
            string actual = planetService.GetName(planet.Id);

            // Assert
            Assert.Equal(planet.Name, actual);
        }

        [Fact]
        public void GetName_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            object actual = planetService.GetName(11);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetForm_WithExistingId_ShouldReturnCorrectResult()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);
            Planet expected = this.GetFakePlanets().FirstOrDefault(p => p.Id == 1);

            // Act
            PlanetFormServiceModel actual = planetService.GetForm(1);

            // Assert
            Assert.True(this.ComparePlanets(expected, actual));
        }

        [Fact]
        public void GetForm_WithNotExistingId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            PlanetFormServiceModel actual = planetService.GetForm(11);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Planets_WithExistingDiscoveryId_ShouldReturnAllPlanets()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            const int discoveryId = 1;

            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                Planets = this.GetFakePlanets()
            };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            // Act
            IEnumerable<ListPlanetsServiceModel> planets = planetService.Planets(discoveryId);

            // Assert
            foreach (var expected in GetFakePlanets())
            {
                ListPlanetsServiceModel actual = planets.FirstOrDefault(p => p.Id == expected.Id);

                Assert.True(this.ComparePlanets(expected, actual));
            }
        }

        [Fact]
        public void Planets_WithNotExistingDiscoveryId_ShouldReturnEmptyCollection()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            const int discoveryId = 1;

            Discovery discovery = new Discovery
            {
                Id = discoveryId,
                Planets = this.GetFakePlanets()
            };

            db.Discoveries.Add(discovery);
            db.SaveChanges();

            // Act
            IEnumerable<ListPlanetsServiceModel> planets = planetService.Planets(2);

            // Assert
            Assert.False(planets.Any());
        }

        private bool ComparePlanets(Planet expected, Planet actual)
        {
            return expected.Id == actual.Id
                && expected.Mass == actual.Mass
                && expected.Name == actual.Name;
        }

        private bool ComparePlanets(Planet expected, PlanetFormServiceModel actual)
        {
            return expected.Mass == actual.Mass
                && expected.Name == actual.Name;
        }

        private bool ComparePlanets(Planet expected, ListPlanetsServiceModel actual)
        {
            return expected.Id == actual.Id
                && expected.Mass == actual.Mass
                && expected.Name == actual.Name;
        }

        private void SeedDatabase(StarStuffDbContext db)
        {
            db.Planets.AddRange(this.GetFakePlanets());
            db.SaveChanges();
        }

        private List<Planet> GetFakePlanets()
        {
            List<Planet> planets = new List<Planet>();

            for (int i = 1; i <= 10; i++)
            {
                planets.Add(new Planet
                {
                    Id = i,
                    Name = $"Planet Name {i}",
                    Mass = i * 2
                });
            }

            return planets;
        }
    }
}