namespace StarStuff.Services.Astronomer.Models.Discoveries
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class DiscoveryServiceModel : IMapFrom<Discovery>
    {
        public int Id { get; set; }

        public string StarSystem { get; set; }
    }
}