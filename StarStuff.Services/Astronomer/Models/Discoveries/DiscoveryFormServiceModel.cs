namespace StarStuff.Services.Astronomer.Models.Discoveries
{
    using Common.Mapping;
    using Data;
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class DiscoveryFormServiceModel : IMapFrom<Telescope>
    {
        [Required]
        [Display(Name = "Star System Name")]
        [StringLength(DataConstants.DiscoveryConstants.StarSystemMaxLength)]
        public string StarSystem { get; set; }
    }
}