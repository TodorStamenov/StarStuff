namespace StarStuff.Web.Areas.Moderator.Models.Publications
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Moderator.Models.Publications;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PublicationFormViewModel
    {
        public PublicationFormServiceModel Publication { get; set; }

        [Display(Name = "Star System Name")]
        public int DiscoveryId { get; set; }

        public IEnumerable<SelectListItem> Discoveries { get; set; }
    }
}