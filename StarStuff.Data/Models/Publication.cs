namespace StarStuff.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Publication
    {
        public int Id { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Content { get; set; }

        public int JournalId { get; set; }

        public Journal Journal { get; set; }

        public int DiscoveryId { get; set; }

        public Discovery Discovery { get; set; }
    }
}