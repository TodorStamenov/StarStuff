namespace StarStuff.Services.Models.Comments
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class CommentFormServiceModel : IMapFrom<Comment>
    {
        [Required]
        public string Content { get; set; }
    }
}