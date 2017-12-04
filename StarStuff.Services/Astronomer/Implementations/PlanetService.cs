namespace StarStuff.Services.Astronomer.Implementations
{
    using StarStuff.Data;

    public class PlanetService
    {
        private readonly StarStuffDbContext db;

        public PlanetService(StarStuffDbContext db)
        {
            this.db = db;
        }
    }
}