namespace StarStuff.Services.Areas.Astronomer.Models.Planets
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    public class PlanetFormServiceModel
    {
        [Required]
        [StringLength(DataConstants.PlanetConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(
            DataConstants.PlanetConstants.MassMinValue,
            DataConstants.PlanetConstants.MassMaxValue)]
        public double Mass { get; set; }
    }
}