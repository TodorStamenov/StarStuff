namespace StarStuff.Services.Moderator.Models.Journals
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;

    public class JournalDetailsServiceModel : ListJournalsServiceModel, ICustomMapping
    {
        public int Publications { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Journal, JournalDetailsServiceModel>()
                .ForMember(j => j.Publications, cfg => cfg.MapFrom(j => j.Publications.Count));
        }
    }
}