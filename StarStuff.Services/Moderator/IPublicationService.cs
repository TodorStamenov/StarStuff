﻿namespace StarStuff.Services.Moderator
{
    using Moderator.Models.Publications;
    using System.Collections.Generic;

    public interface IPublicationService
    {
        int Total();

        int TotalByJournal(int journalId);

        int TotalByTelescope(int telescopeId);

        int Create(string content, int discoveryId, int journalId);

        bool Edit(int id, string content);

        PublicationFormServiceModel GetForm(int id);

        PublicationDetailsServiceModel Details(int id);

        IEnumerable<ListPublicationsServiceModel> All(int page, int pageSize);

        IEnumerable<ListPublicationsServiceModel> AllByJournal(int journalId, int page, int pageSize);

        IEnumerable<ListPublicationsServiceModel> AllByTelescope(int telescopeId, int page, int pageSize);
    }
}