namespace StarStuff.Services.Moderator.Models.Telescopes
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System.Linq;

    public class TelescopeDetailsServiceModel : ListTelescopesServiceModel, ICustomMapping
    {
        public string Location { get; set; }

        public double MirrorDiameter { get; set; }

        public int Discoveries { get; set; }

        public int Publications { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Telescope, TelescopeDetailsServiceModel>()
                .ForMember(t => t.Discoveries,
                    cfg => cfg.MapFrom(t => t.Discoveries.Count(d => d.IsConfirmed)))
                .ForMember(t => t.Publications,
                    cfg => cfg.MapFrom(t => t.Discoveries.Sum(d => d.Publications.Count)));
        }
    }
}