namespace StarStuff.Services.Moderator.Models.Publications
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;

    public class ListPublicationsServiceModel : IMapFrom<Publication>, ICustomMapping
    {
        public int Id { get; set; }

        public string StarSystemName { get; set; }

        public string Content { get; set; }

        public int JournalId { get; set; }

        public string JournalName { get; set; }

        public int CommentsCount { get; set; }

        public virtual void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Publication, ListPublicationsServiceModel>()
               .ForMember(p => p.StarSystemName, cfg => cfg.MapFrom(p => p.Discovery.StarSystem))
               .ForMember(p => p.CommentsCount, cfg => cfg.MapFrom(p => p.Comments.Count))
               .ForMember(p => p.JournalName, cfg => cfg.MapFrom(p => p.Journal.Name));
        }
    }
}