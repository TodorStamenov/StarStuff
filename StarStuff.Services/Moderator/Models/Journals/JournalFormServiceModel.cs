namespace StarStuff.Services.Moderator.Models.Journals
{
    using Common.Mapping;
    using Data;
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class JournalFormServiceModel : IMapFrom<Journal>
    {
        [Required]
        [StringLength(DataConstants.JournalConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

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