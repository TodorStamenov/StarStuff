namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Stars;
    using StarStuff.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class StarService : IStarService
    {
        private readonly StarStuffDbContext db;

        public StarService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ListStarsServiceModel> Stars(int discoveryId)
        {
            return this.db
                .Stars
                .Where(s => s.DiscoveryId == discoveryId)
                .ProjectTo<ListStarsServiceModel>()
                .ToList();
        }
    }
}