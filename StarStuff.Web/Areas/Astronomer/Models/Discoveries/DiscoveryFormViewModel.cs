namespace StarStuff.Web.Areas.Astronomer.Models.Discoveries
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using StarStuff.Services.Astronomer.Models.Discoveries;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DiscoveryFormViewModel
    {
        public DiscoveryFormServiceModel Discovery { get; set; }

        [Display(Name = "Telescope")]
        public int TelescopeId { get; set; }

        public IEnumerable<SelectListItem> Telescopes { get; set; }
    }
}