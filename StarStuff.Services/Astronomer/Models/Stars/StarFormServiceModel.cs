namespace StarStuff.Services.Astronomer.Models.Stars
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    public class StarFormServiceModel
    {
        [Required]
        [StringLength(DataConstants.StarConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(
            DataConstants.StarConstants.TemperatureMinValue,
            DataConstants.StarConstants.TemperatureMaxValue)]
        public int Temperature { get; set; }
    }
}