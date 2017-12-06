namespace StarStuff.Services.Moderator.Models.Telescopes
{
    using AutoMapper;
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class TelescopeDetailsServiceModel : ListTelescopesServiceModel, ICustomMapping
    {
        public string Location { get; set; }

        public double MirrorDiameter { get; set; }

        public int Discoveries { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Telescope, TelescopeDetailsServiceModel>()
                .ForMember(t => t.Discoveries, cfg => cfg.MapFrom(t => t.Discoveries.Count));
        }
    }
}