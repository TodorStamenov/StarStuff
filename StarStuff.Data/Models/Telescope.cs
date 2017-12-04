﻿namespace StarStuff.Data.Models
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

        [Required]
        public string Description { get; set; }

        [Range(DataConstants.TelescopeConstants.MinMirrorDiameter, double.MaxValue)]
        public double MirrorDiameter { get; set; }

        [Required]
        [MinLength(DataConstants.ImageUrlMinLength)]
        [MaxLength(DataConstants.ImageUrlMaxLength)]
        [RegularExpression(DataConstants.ImageUrlPattern)]
        public string ImageUrl { get; set; }
    }
}