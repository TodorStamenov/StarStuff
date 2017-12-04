namespace StarStuff.Services.Astronomer.Implementations
{
    using StarStuff.Data;

    public class DiscoveryService : IDiscoveryService
    {
        private readonly StarStuffDbContext db;

        public DiscoveryService(StarStuffDbContext db)
        {
            this.db = db;
        }
    }
}