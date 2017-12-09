namespace StarStuff.Services.Moderator
{
    using Models.Journals;
    using System.Collections.Generic;

    public interface IJournalService
    {
        bool Exists(string name);

        int Total();

        int Create(
            string name,
            string description,
            string imageUrl);

        bool Edit(
            int id,
            string name,
            string description,
            string imageUrl);

        string GetName(int journalId);

        JournalFormServiceModel GetForm(int id);

        JournalDetailsServiceModel Details(int id);

        IEnumerable<ListJournalsServiceModel> All(int page, int pageSize);
    }
}