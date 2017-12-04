namespace StarStuff.Services.Moderator.Models.Telescopes
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class ListTelescopesServiceModel : IMapFrom<Telescope>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}