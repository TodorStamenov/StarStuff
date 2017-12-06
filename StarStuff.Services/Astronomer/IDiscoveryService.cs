namespace StarStuff.Services.Astronomer
{
    using Models.Discoveries;
    using System.Collections.Generic;

    public interface IDiscoveryService
    {
        IEnumerable<DiscoveryServiceModel> DiscoveryDropdown(int journalId);
    }
}