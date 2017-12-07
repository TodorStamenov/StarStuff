namespace StarStuff.Services.Astronomer
{
    using Models.Planets;
    using System.Collections.Generic;

    public interface IPlanetService
    {
        IEnumerable<ListPlanetsServiceModel> Planets(int discoveryId);
    }
}