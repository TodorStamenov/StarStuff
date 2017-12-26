namespace StarStuff.Services.Areas.Astronomer
{
    using Models.Planets;
    using System.Collections.Generic;

    public interface IPlanetService
    {
        bool Exists(string name);

        bool Create(int discoveryId, string name, double mass);

        bool Edit(int id, string name, double mass);

        bool Delete(int id);

        string GetName(int id);

        PlanetFormServiceModel GetForm(int id);

        IEnumerable<ListPlanetsServiceModel> Planets(int discoveryId);
    }
}