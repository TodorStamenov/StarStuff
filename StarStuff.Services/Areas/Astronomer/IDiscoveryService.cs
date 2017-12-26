namespace StarStuff.Services.Areas.Astronomer
{
    using Models.Astronomers;
    using Models.Discoveries;
    using System.Collections.Generic;

    public interface IDiscoveryService
    {
        bool Exists(string starSystem);

        bool IsPioneer(int discoveryId, int pioneerId);

        bool IsObserver(int discoveryId, int observerId);

        int TotalStars(int discoveryId);

        int Total(string search, bool? confirmed);

        int Total(string search, bool? confirmed, int astronomerId, AstronomerType astronomerType);

        int Create(string starSystem, long distance, int telescopeId, int astronomerId, IEnumerable<int> astronomerIds);

        bool Edit(int id, string starSystem, long distance);

        bool Delete(int id);

        bool Confirm(int discoveryId, int astronomerId);

        string GetName(int id);

        DiscoveryFormServiceModel GetForm(int id);

        DiscoveryDetailsServiceModel Details(int id);

        IEnumerable<AstronomerServiceModel> Observers(int discoveryId);

        IEnumerable<AstronomerServiceModel> Pioneers(int discoveryId);

        IEnumerable<DiscoveryDropdownServiceModel> DiscoveryDropdown(int journalId);

        IEnumerable<ListDiscoveriesServiceModel> All(int page, int pageSize, string search, bool? confirmed);

        IEnumerable<ListDiscoveriesServiceModel> All(int page, int pageSize, string search, bool? confirmed, int astronomerId, AstronomerType astronomerType);
    }
}