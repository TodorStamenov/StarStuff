namespace StarStuff.Services.Areas.Astronomer.Models.Discoveries
{
    using Common.Mapping;
    using Data.Models;

    public class DiscoveryDropdownServiceModel : IMapFrom<Discovery>
    {
        public int Id { get; set; }

        public string StarSystem { get; set; }
    }
}