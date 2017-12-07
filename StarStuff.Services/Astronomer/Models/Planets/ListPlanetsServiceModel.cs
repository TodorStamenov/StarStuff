namespace StarStuff.Services.Astronomer.Models.Planets
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class ListPlanetsServiceModel : IMapFrom<Planet>
    {
        public string Name { get; set; }

        public double Mass { get; set; }
    }
}