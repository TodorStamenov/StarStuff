namespace StarStuff.Web.Models.Publications
{
    using Infrastructure.Helpers;
    using Services.Models.Comments;
    using Services.Moderator.Models.Publications;
    using System.Collections.Generic;

    public class PublicationDetailsViewModel : BasePageViewModel
    {
        public PublicationDetailsServiceModel Publication { get; set; }

        public IEnumerable<ListCommentsServiceModel> Comments { get; set; }
    }
}