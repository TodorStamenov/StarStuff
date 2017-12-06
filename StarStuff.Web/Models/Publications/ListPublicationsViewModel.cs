namespace StarStuff.Web.Models.Publications
{
    using StarStuff.Services.Moderator.Models.Publications;
    using System.Collections.Generic;

    public class ListPublicationsViewModel : BasePageViewModel
    {
        public IEnumerable<ListPublicationsServiceModel> Publications { get; set; }
    }
}