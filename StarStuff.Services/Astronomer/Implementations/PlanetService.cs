namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Planets;
    using StarStuff.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class PlanetService : IPlanetService
    {
        private readonly StarStuffDbContext db;

        public PlanetService(StarStuffDbContext db)
        {
            this.db = db;
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