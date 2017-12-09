namespace StarStuff.Services.Moderator.Models.Journals
{
    using Common.Mapping;
    using Data.Models;

    public class JournalServiceModel : IMapFrom<Journal>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}