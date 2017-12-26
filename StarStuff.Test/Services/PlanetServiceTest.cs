namespace StarStuff.Test.Services
{
    using Data;
    using Data.Models;
    using StarStuff.Services.Areas.Astronomer.Implementations;
    using StarStuff.Services.Areas.Astronomer.Models.Planets;
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
            string name = this.GetFakePlanets().First().Name;
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

            string name = this.GetFakePlanets().First().Name;

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
            this.SeedDiscovery(db, true);

            Planet expected = this.GetFakePlanets().First();

            // Act
            bool result = planetService.Create(discoveryId, expected.Name, expected.Mass);

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
            this.SeedDiscovery(db, true);

            Planet expected = this.GetFakePlanets().First();

            // Act
            planetService.Create(discoveryId, expected.Name, expected.Mass);
            Planet actual = db.Planets.First();

            // Assert
            Assert.Equal(1, db.Planets.Count());
            this.ComparePlanets(expected, actual);
        }

        [Fact]
        public void Create_WithExistingName_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            const int discoveryId = 1;
            this.SeedDiscovery(db, true);

            Planet planet = this.GetFakePlanets().First();

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

            Planet planet = this.GetFakePlanets().First();

            // Act
            bool result = planetService.Create(1, planet.Name, planet.Mass);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithExistingDicoveryWithoutStars_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDiscovery(db, false);

            Planet planet = this.GetFakePlanets().First();

            // Act
            bool result = planetService.Create(1, planet.Name, planet.Mass);

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

            Planet planet = this.GetFakePlanets().First();

            // Act
            bool result = planetService.Edit(planet.Id, planet.Name, planet.Mass);

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
            Planet actual = db.Planets.Find(planetId);

            // Assert
            this.ComparePlanets(expected, actual);
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

            Planet planet = this.GetFakePlanets().First();
            planet.Mass = 3.14;

            // Act
            bool result = planetService.Edit(planet.Id, planet.Name, planet.Mass);

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

            Planet expected = this.GetFakePlanets().First();
            expected.Mass = 3.14;

            // Act
            planetService.Edit(expected.Id, expected.Name, expected.Mass);
            Planet actual = db.Planets.Find(expected.Id);

            // Assert
            this.ComparePlanets(expected, actual);
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
            int planetsCount = this.GetFakePlanets().Count;
            const int planetId = 1;
            this.SeedDatabase(db);

            // Act
            planetService.Delete(planetId);
            Planet planet = db.Planets.Find(planetId);

            // Assert
            Assert.Null(planet);
            Assert.Equal(planetsCount - 1, db.Planets.Count());
        }

        [Fact]
        public void Delete_WithNotExistingId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            int planetsCount = this.GetFakePlanets().Count;
            this.SeedDatabase(db);

            // Act
            bool result = planetService.Delete(planetsCount + 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_WithNotExistingId_ShouldNotRemovePlanet()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            int planetsCount = this.GetFakePlanets().Count;
            this.SeedDatabase(db);

            // Act
            planetService.Delete(11);

            // Assert
            Assert.Equal(planetsCount, db.Planets.Count());
        }

        [Fact]
        public void GetName_WithExistingId_ShouldReturnValidName()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);
            this.SeedDatabase(db);

            // Act
            Planet planet = this.GetFakePlanets().First();
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
            Planet expected = this.GetFakePlanets().First();

            // Act
            PlanetFormServiceModel actual = planetService.GetForm(expected.Id);

            // Assert
            this.ComparePlanets(expected, actual);
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

            this.SeedDiscovery(db, false, true);

            List<Planet> fakePlanets = this.GetFakePlanets();

            int i = -1;

            // Act
            IEnumerable<ListPlanetsServiceModel> planets = planetService.Planets(discoveryId);

            // Assert
            foreach (var actual in planets)
            {
                Planet expected = fakePlanets[++i];

                this.ComparePlanets(expected, actual);
            }
        }

        [Fact]
        public void Planets_WithNotExistingDiscoveryId_ShouldReturnEmptyCollection()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            PlanetService planetService = new PlanetService(db);

            this.SeedDiscovery(db, false, true);

            // Act
            IEnumerable<ListPlanetsServiceModel> planets = planetService.Planets(2);

            // Assert
            Assert.False(planets.Any());
        }

        private void ComparePlanets(Planet expected, Planet actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Mass, actual.Mass);
            Assert.Equal(expected.Name, actual.Name);
        }

        private void ComparePlanets(Planet expected, PlanetFormServiceModel actual)
        {
            Assert.Equal(expected.Mass, actual.Mass);
            Assert.Equal(expected.Name, actual.Name);
        }

        private void ComparePlanets(Planet expected, ListPlanetsServiceModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Mass, actual.Mass);
            Assert.Equal(expected.Name, actual.Name);
        }

        private void SeedDatabase(StarStuffDbContext db)
        {
            db.Planets.AddRange(this.GetFakePlanets());
            db.SaveChanges();
        }

        private void SeedDiscovery(StarStuffDbContext db, bool withStar, bool withPlanets = false)
        {
            Discovery discovery = new Discovery { Id = 1 };

            db.Discoveries.Add(discovery);

            if (withStar)
            {
                discovery.Stars.Add(new Star { Id = 1 });
            }

            if (withPlanets)
            {
                discovery.Planets.AddRange(this.GetFakePlanets());
            }

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

            return planets.OrderBy(p => p.Id).ToList();
        }
    }
}