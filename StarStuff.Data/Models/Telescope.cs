namespace StarStuff.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Telescope
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.TelescopeConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataConstants.TelescopeConstants.LocationMaxLength)]
        public string Location { get; set; }

        [Range(0, double.MaxValue)]
        public double MirrorDiameter { get; set; }

        [Required]
        [MinLength(DataConstants.TelescopeConstants.ImageUrlMinLength)]
        [RegularExpression(DataConstants.TelescopeConstants.ImageUrlPattern)]
        public string ImageUrl { get; set; }
    }
}