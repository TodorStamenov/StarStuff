namespace StarStuff.Services.Moderator
{
    using Models.Publications;
    using System.Collections.Generic;

    public interface IPublicationService
    {
        bool Exists(int journalId, int discoveryId);

        bool TitleExists(string title);

        string GetTitle(int id);

        int Total();

        int TotalByJournal(int journalId);

        int TotalByTelescope(int telescopeId);

        int Create(string title, string content, int discoveryId, int journalId, int authorId);

        bool Edit(int id, string title, string content);

        PublicationFormServiceModel GetForm(int id);

        PublicationDetailsServiceModel Details(int id);

        IEnumerable<ListPublicationsServiceModel> All(int page, int pageSize);

        IEnumerable<ListPublicationsServiceModel> AllByJournal(int journalId, int page, int pageSize);

        IEnumerable<ListPublicationsServiceModel> AllByTelescope(int telescopeId, int page, int pageSize);
    }
}