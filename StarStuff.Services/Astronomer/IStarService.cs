namespace StarStuff.Services.Astronomer
{
    using Models.Stars;
    using System.Collections.Generic;

    public interface IStarService
    {
        IEnumerable<ListStarsServiceModel> Stars(int discoveryId);
    }
}