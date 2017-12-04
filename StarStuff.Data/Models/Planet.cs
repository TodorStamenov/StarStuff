namespace StarStuff.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Planet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.PlanetConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(
            DataConstants.PlanetConstants.MassMinValue,
            DataConstants.PlanetConstants.MassMaxValue)]
        public double Mass { get; set; }

        public int DiscoveryId { get; set; }

        public Discovery Discovery { get; set; }
    }
}