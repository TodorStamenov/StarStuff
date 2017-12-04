namespace StarStuff.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Journal
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.JournalConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MinLength(DataConstants.ImageUrlMinLength)]
        [MaxLength(DataConstants.ImageUrlMaxLength)]
        [RegularExpression(DataConstants.ImageUrlPattern)]
        public string ImageUrl { get; set; }
    }
}