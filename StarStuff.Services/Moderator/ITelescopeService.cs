﻿namespace StarStuff.Services.Moderator
{
    using Models.Telescopes;
    using System.Collections.Generic;

    public interface ITelescopeService
    {
        int Total();

        string GetName(int telescopeId);

        int Create(
            string name,
            string location,
            string description,
            double mirrorDiameter,
            string imageUrl);

        bool Edit(
            int id,
            string name,
            string location,
            string description,
            double mirrorDiameter,
            string imageUrl);

        TelescopeFormServiceModel GetForm(int id);

        TelescopeDetailsServiceModel Details(int id);

        IEnumerable<ListTelescopesServiceModel> All(int page, int pageSize);
        IEnumerable<TelescopeServiceModel> TelescopeDropdown();
    }
}