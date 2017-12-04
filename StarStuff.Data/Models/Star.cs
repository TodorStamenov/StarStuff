namespace StarStuff.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Star
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.StarConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(
            DataConstants.StarConstants.TemperatureMinValue,
            DataConstants.StarConstants.TemperatureMaxValue)]
        public int Temperature { get; set; }

        public int DiscoveryId { get; set; }

        public Discovery Discovery { get; set; }
    }
}