namespace StarStuff.Web.Areas.Astronomer.Models.Discoveries
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Astronomer.Models.Discoveries;
    using Services.Astronomer.Models.Stars;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DiscoveryFormViewModel
    {
        public DiscoveryFormServiceModel Discovery { get; set; }

        public StarFormServiceModel Star { get; set; }

        [Display(Name = "Telescope")]
        public int TelescopeId { get; set; }

        [Display(Name = "Discoverers")]
        public IEnumerable<int> AstronomerIds { get; set; }

        public IEnumerable<SelectListItem> Telescopes { get; set; }

        public IEnumerable<SelectListItem> Astronomers { get; set; }
    }
}