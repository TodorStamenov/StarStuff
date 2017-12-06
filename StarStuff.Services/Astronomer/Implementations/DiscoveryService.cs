namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Discoveries;
    using StarStuff.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class DiscoveryService : IDiscoveryService
    {
        private readonly StarStuffDbContext db;

        public DiscoveryService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<DiscoveryServiceModel> DiscoveryDropdown(int journalId)
        {
            return this.db
                .Discoveries
                .Where(d => d.IsConfirmed)
                .Where(d => !d.Publications.Any(p => p.JournalId == journalId))
                .OrderByDescending(d => d.DateMade)
                .ProjectTo<DiscoveryServiceModel>()
                .ToList();
        }
    }
}