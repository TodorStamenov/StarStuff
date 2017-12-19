namespace StarStuff.Services.Astronomer.Models.Discoveries
{
    using Common.Mapping;
    using Data;
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class DiscoveryFormServiceModel : IMapFrom<Discovery>
    {
        [Required]
        [Display(Name = "Star System Name")]
        [StringLength(DataConstants.DiscoveryConstants.StarSystemMaxLength)]
        public string StarSystem { get; set; }

        [Display(Name = "Distance from Earth in light years")]
        [Range(
            DataConstants.DiscoveryConstants.MinStarSystemDistance,
            DataConstants.DiscoveryConstants.MaxStarSystemDistance)]
        public long Distance { get; set; }
    }
}