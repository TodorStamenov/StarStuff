namespace StarStuff.Services.Moderator.Implementations
{
    using StarStuff.Data;

    public class PublicationService : IPublicationService
    {
        private readonly StarStuffDbContext db;

        public PublicationService(StarStuffDbContext db)
        {
            this.db = db;
        }
    }
}