namespace StarStuff.Services.Areas.Moderator.Models.Telescopes
{
    using Common.Mapping;
    using Data;
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class TelescopeFormServiceModel : IMapFrom<Telescope>
    {
        [Required]
        [StringLength(DataConstants.TelescopeConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DataConstants.TelescopeConstants.LocationMaxLength)]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Mirror Diameter in Meters")]
        [Range(DataConstants.TelescopeConstants.MinMirrorDiameter, double.MaxValue)]
        public double MirrorDiameter { get; set; }

        [Display(Name = "Image URL")]
        [Required]
        [StringLength(
            DataConstants.ImageUrlMaxLength,
            MinimumLength = DataConstants.ImageUrlMinLength)]
        [RegularExpression(
            DataConstants.ImageUrlPattern,
            ErrorMessage = DataConstants.InvalidUrlFormatMessage)]
        public string ImageUrl { get; set; }
    }
}