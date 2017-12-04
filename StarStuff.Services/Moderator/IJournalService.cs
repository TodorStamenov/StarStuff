namespace StarStuff.Services.Moderator
{
    using Models.Journals;
    using System.Collections.Generic;

    public interface IJournalService
    {
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

        JournalFormServiceModel GetForm(int id);

        JournalDetailsServiceModel Details(int id);

        IEnumerable<ListJournalsServiceModel> All(int page, int pageSize);
    }
}