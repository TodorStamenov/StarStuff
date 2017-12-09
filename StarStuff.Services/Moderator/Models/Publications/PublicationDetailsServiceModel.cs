namespace StarStuff.Services.Moderator.Models.Publications
{
    using AutoMapper;
    using Data.Models;
    using System;

    public class PublicationDetailsServiceModel : ListPublicationsServiceModel
    {
        public DateTime ReleaseDate { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Publication, PublicationDetailsServiceModel>()
                .ForMember(p => p.StarSystemName, cfg => cfg.MapFrom(p => p.Discovery.StarSystem))
                .ForMember(p => p.CommentsCount, cfg => cfg.MapFrom(p => p.Comments.Count))
                .ForMember(p => p.JournalName, cfg => cfg.MapFrom(p => p.Journal.Name));
        }
    }
}