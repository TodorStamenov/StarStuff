namespace StarStuff.Services.Astronomer.Models.Discoveries
{
    using Common.Mapping;
    using Data.Models;

    public class DiscoveryServiceModel : IMapFrom<Discovery>
    {
        public int Id { get; set; }

        public string StarSystem { get; set; }
    }
}