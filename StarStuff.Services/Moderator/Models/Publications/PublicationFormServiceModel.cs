namespace StarStuff.Services.Moderator.Models.Publications
{
    using System.ComponentModel.DataAnnotations;

    public class PublicationFormServiceModel
    {
        [Required]
        public string Content { get; set; }
    }
}