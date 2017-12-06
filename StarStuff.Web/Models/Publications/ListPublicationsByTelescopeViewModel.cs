namespace StarStuff.Web.Models.Publications
{
    public class ListPublicationsByTelescopeViewModel : ListPublicationsViewModel
    {
        public int TelescopeId { get; set; }

        public string TelescopeName { get; set; }
    }
}