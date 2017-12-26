namespace StarStuff.Services.Areas.Astronomer.Models.Discoveries
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Linq;

    public class ListDiscoveriesServiceModel : DiscoveryServiceModel, ICustomMapping
    {
        public int Stars { get; set; }

        public int Planets { get; set; }

        public bool IsConfirmed { get; set; }

        public bool HasPublication { get; set; }

        public DateTime DateMade { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Discovery, ListDiscoveriesServiceModel>()
                .ForMember(d => d.HasPublication, cfg => cfg.MapFrom(d => d.Publications.Any()))
                .ForMember(d => d.Stars, cfg => cfg.MapFrom(d => d.Stars.Count))
                .ForMember(d => d.Planets, cfg => cfg.MapFrom(d => d.Planets.Count));
        }
    }
}