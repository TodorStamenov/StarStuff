namespace StarStuff.Services.Moderator.Models.Journals
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class JournalServiceModel : IMapFrom<Journal>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}