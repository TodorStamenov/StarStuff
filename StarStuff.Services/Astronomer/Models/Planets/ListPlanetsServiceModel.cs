namespace StarStuff.Services.Astronomer.Models.Planets
{
    using Common.Mapping;
    using Data.Models;

    public class ListPlanetsServiceModel : IMapFrom<Planet>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Mass { get; set; }
    }
}