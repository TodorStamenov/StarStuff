namespace StarStuff.Web.Models.Publications
{
    using Infrastructure;
    using Services.Moderator.Models.Publications;
    using System.Collections.Generic;

    public class ListPublicationsViewModel : BasePageViewModel
    {
        public IEnumerable<ListPublicationsServiceModel> Publications { get; set; }
    }
}