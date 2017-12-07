namespace StarStuff.Services.Moderator.Models.Telescopes
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class TelescopeServiceModel : IMapFrom<Telescope>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}