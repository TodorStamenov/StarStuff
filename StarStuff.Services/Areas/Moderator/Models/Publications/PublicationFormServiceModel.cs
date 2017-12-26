namespace StarStuff.Services.Areas.Moderator.Models.Publications
{
    using StarStuff.Data;
    using System.ComponentModel.DataAnnotations;

    public class PublicationFormServiceModel
    {
        [Required]
        [StringLength(DataConstants.PublicationConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}