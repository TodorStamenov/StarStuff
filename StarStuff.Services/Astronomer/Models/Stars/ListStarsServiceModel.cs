namespace StarStuff.Services.Astronomer.Models.Stars
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class ListStarsServiceModel : IMapFrom<Star>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Temperature { get; set; }
    }
}