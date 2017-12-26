namespace StarStuff.Services.Areas.Astronomer.Models.Stars
{
    using Common.Mapping;
    using Data.Models;

    public class ListStarsServiceModel : IMapFrom<Star>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Temperature { get; set; }
    }
}