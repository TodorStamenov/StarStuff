namespace StarStuff.Services.Areas.Astronomer.Models.Stars
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    public class StarFormServiceModel
    {
        [Required]
        [Display(Name = "Star Name")]
        [StringLength(DataConstants.StarConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(
            DataConstants.StarConstants.TemperatureMinValue,
            DataConstants.StarConstants.TemperatureMaxValue)]
        public int Temperature { get; set; }
    }
}