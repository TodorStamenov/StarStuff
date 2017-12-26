namespace StarStuff.Services.Areas.Moderator.Models.Telescopes
{
    using Common.Mapping;
    using Data.Models;

    public class TelescopeServiceModel : IMapFrom<Telescope>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}