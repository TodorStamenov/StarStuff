﻿namespace StarStuff.Services.Astronomer
{
    using Models.Astronomers;
    using Models.Discoveries;
    using System.Collections.Generic;

    public interface IDiscoveryService
    {
        bool Exists(string starSystem);

        bool IsPioneer(int discoveryId, int pioneerId);

        bool IsObserver(int discoveryId, int observerId);

        int Total(bool? confirmed);

        int Total(bool? confirmed, AstronomerType astronomerType, int astronomerId);

        int Create(string starSystem, int telescopeId, int astronomerId, IEnumerable<int> astronomerIds);

        bool Edit(int id, string starSystem);

        bool Delete(int id);

        bool Confirm(int discoveryId, int userId);

        string GetName(int id);

        DiscoveryFormServiceModel GetForm(int id);

        DiscoveryDetailsServiceModel Details(int id);

        IEnumerable<AstronomerServiceModel> Observers(int discoveryId);

        IEnumerable<AstronomerServiceModel> Pioneers(int discoveryId);

        IEnumerable<DiscoveryServiceModel> DiscoveryDropdown(int journalId);

        IEnumerable<ListDiscoveriesServiceModel> All(int page, int pageSize, bool? confirmed = null);

        IEnumerable<ListDiscoveriesServiceModel> All(int page, int pageSize, int astronomerId, AstronomerType astronomerType, bool? confirmed);
    }
}