namespace StarStuff.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Publication
    {
        public int Id { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        [MaxLength(DataConstants.PublicationConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int Views { get; set; }

        public int JournalId { get; set; }

        public Journal Journal { get; set; }

        public int DiscoveryId { get; set; }

        public Discovery Discovery { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}