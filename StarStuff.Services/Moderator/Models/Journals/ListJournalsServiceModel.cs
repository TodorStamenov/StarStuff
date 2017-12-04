namespace StarStuff.Services.Moderator.Models.Journals
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class ListJournalsServiceModel : IMapFrom<Journal>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}