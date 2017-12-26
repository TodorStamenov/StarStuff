namespace StarStuff.Services.Areas.Moderator.Models.Publications
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;

    public class PublicationDetailsServiceModel : IMapFrom<Publication>, ICustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string StarSystemName { get; set; }

        public long Distance { get; set; }

        public int TelescopeId { get; set; }

        public string TelescopeName { get; set; }

        public int JournalId { get; set; }

        public string JournalName { get; set; }

        public string AuthorName { get; set; }

        public DateTime ReleaseDate { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Publication, PublicationDetailsServiceModel>()
                .ForMember(p => p.StarSystemName, cfg => cfg.MapFrom(p => p.Discovery.StarSystem))
                .ForMember(p => p.Distance, cfg => cfg.MapFrom(p => p.Discovery.Distance))
                .ForMember(p => p.TelescopeName, cfg => cfg.MapFrom(p => p.Discovery.Telescope.Name))
                .ForMember(p => p.TelescopeId, cfg => cfg.MapFrom(p => p.Discovery.TelescopeId))
                .ForMember(p => p.JournalName, cfg => cfg.MapFrom(p => p.Journal.Name))
                .ForMember(p => p.AuthorName, cfg => cfg.MapFrom(p => p.Author.UserName));
        }
    }
}