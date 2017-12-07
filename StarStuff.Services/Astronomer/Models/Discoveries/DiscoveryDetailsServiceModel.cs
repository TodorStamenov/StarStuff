namespace StarStuff.Services.Astronomer.Models.Discoveries
{
    using AutoMapper;
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;
    using System;

    public class DiscoveryDetailsServiceModel : DiscoveryServiceModel, ICustomMapping
    {
        public int TelescopeId { get; set; }

        public string TelescopeName { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsConfirmedByUser { get; set; }

        public DateTime DateMade { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Discovery, DiscoveryDetailsServiceModel>()
                .ForMember(d => d.TelescopeName, cfg => cfg.MapFrom(d => d.Telescope.Name));
        }
    }
}