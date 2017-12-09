namespace StarStuff.Services.Models.Comments
{
    using Common.Mapping;
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class CommentFormServiceModel : IMapFrom<Comment>
    {
        [Required]
        public string Content { get; set; }
    }
}