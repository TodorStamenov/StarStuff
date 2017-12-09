namespace StarStuff.Services.Astronomer
{
    using Models.Stars;
    using System.Collections.Generic;

    public interface IStarService
    {
        bool Exists(string name);

        bool Create(int discoveryId, string name, int temperature);

        bool Edit(int id, string name, int temperature);

        bool Delete(int id);

        string GetName(int id);

        StarFormServiceModel GetForm(int id);

        IEnumerable<ListStarsServiceModel> Stars(int discoveryId);
    }
}