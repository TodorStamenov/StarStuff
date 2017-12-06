namespace StarStuff.Web.Models.Publications
{
    public class ListPublicationsByJournalViewModel : ListPublicationsViewModel
    {
        public int JournalId { get; set; }

        public string JournalName { get; set; }
    }
}