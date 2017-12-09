namespace StarStuff.Services.Models.Astronomers
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;

    public class ListAstronomersServiceModel : IMapFrom<User>, ICustomMapping
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileImage { get; set; }

        public int DiscoveriesCount { get; set; }

        public int ObservationsCount { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<User, ListAstronomersServiceModel>()
                .ForMember(a => a.DiscoveriesCount, cfg => cfg.MapFrom(a => a.Discoveries.Count))
                .ForMember(a => a.ObservationsCount, cfg => cfg.MapFrom(a => a.Observations.Count))
                .ForMember(a => a.ProfileImage,
                    cfg => cfg.MapFrom(a => "data:image/jpeg;base64," + Convert.ToBase64String(a.ProfileImage)));
        }
    }
}