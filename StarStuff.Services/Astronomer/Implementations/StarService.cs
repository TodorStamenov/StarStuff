namespace StarStuff.Services.Astronomer.Implementations
{
    using StarStuff.Data;

    public class StarService : IStarService
    {
        private readonly StarStuffDbContext db;

        public StarService(StarStuffDbContext db)
        {
            this.db = db;
        }
    }
}