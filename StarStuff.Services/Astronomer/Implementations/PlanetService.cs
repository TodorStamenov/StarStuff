namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Planets;
    using System.Collections.Generic;
    using System.Linq;

    public class PlanetService : IPlanetService
    {
        private readonly StarStuffDbContext db;

        public PlanetService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public bool Exists(string name)
        {
            return this.db.Planets.Any(p => p.Name == name);
        }

        public bool Create(int discoveryId, string name, double mass)
        {
            var discoveryInfo = this.db
                .Discoveries
                .Where(d => d.Id == discoveryId)
                .Select(d => new
                {
                    HasStars = d.Stars.Any()
                })
                .FirstOrDefault();

            bool hasPlanet = this.db.Planets.Any(p => p.Name == name);

            if (discoveryInfo == null
                || hasPlanet
                || !discoveryInfo.HasStars)
            {
                return false;
            }

            Planet planet = new Planet
            {
                DiscoveryId = discoveryId,
                Name = name,
                Mass = mass
            };

            this.db.Planets.Add(planet);
            this.db.SaveChanges();

            return true;
        }

        public bool Edit(int id, string name, double mass)
        {
            Planet planet = this.db.Planets.Find(id);

            if (planet == null
                || (planet.Name != name
                    && this.db.Planets.Any(p => p.Name == name)))
            {
                return false;
            }

            planet.Name = name;
            planet.Mass = mass;

            this.db.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Planet planet = this.db.Planets.Find(id);

            if (planet == null)
            {
                return false;
            }

            this.db.Planets.Remove(planet);
            this.db.SaveChanges();

            return true;
        }

        public string GetName(int id)
        {
            return this.db
                .Planets
                .Where(p => p.Id == id)
                .Select(p => p.Name)
                .FirstOrDefault();
        }

        public PlanetFormServiceModel GetForm(int id)
        {
            return this.db
                .Planets
                .Where(p => p.Id == id)
                .ProjectTo<PlanetFormServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListPlanetsServiceModel> Planets(int discoveryId)
        {
            return this.db
                .Planets
                .Where(p => p.DiscoveryId == discoveryId)
                .ProjectTo<ListPlanetsServiceModel>()
                .ToList();
        }
    }
}