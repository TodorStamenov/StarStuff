namespace StarStuff.Services.Moderator.Implementations
{
    using StarStuff.Data;

    public class TelescopeService : ITelescopeService
    {
        private readonly StarStuffDbContext db;

        public TelescopeService(StarStuffDbContext db)
        {
            this.db = db;
        }
    }
}