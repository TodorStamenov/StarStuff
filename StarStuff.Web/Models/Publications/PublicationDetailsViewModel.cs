namespace StarStuff.Web.Models.Publications
{
    using StarStuff.Services.Models.Comments;
    using StarStuff.Services.Moderator.Models.Publications;
    using System.Collections.Generic;

    public class PublicationDetailsViewModel : BasePageViewModel
    {
        public PublicationDetailsServiceModel Publication { get; set; }

        public IEnumerable<ListCommentsServiceModel> Comments { get; set; }
    }
}