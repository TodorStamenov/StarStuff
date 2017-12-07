namespace StarStuff.Services.Astronomer.Models.Discoveries
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class DiscoveryFormServiceModel : IMapFrom<Telescope>
    {
        [Required]
        [Display(Name = "Star System Name")]
        [StringLength(DataConstants.DiscoveryConstants.StarSystemMaxLength)]
        public string StarSystem { get; set; }
    }
}