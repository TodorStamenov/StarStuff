namespace StarStuff.Services.Areas.Astronomer.Models.Discoveries
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Linq;

    public class DiscoveryDetailsServiceModel : DiscoveryServiceModel, ICustomMapping
    {
        public int TelescopeId { get; set; }

        public string TelescopeName { get; set; }

        public bool IsConfirmed { get; set; }

        public bool HasPublication { get; set; }

        public DateTime DateMade { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Discovery, DiscoveryDetailsServiceModel>()
                .ForMember(d => d.HasPublication, cfg => cfg.MapFrom(d => d.Publications.Any()))
                .ForMember(d => d.TelescopeName, cfg => cfg.MapFrom(d => d.Telescope.Name));
        }
    }
}