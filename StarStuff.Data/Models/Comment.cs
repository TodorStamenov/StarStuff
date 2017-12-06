namespace StarStuff.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DateAdded { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PublicationId { get; set; }

        public Publication Publication { get; set; }
    }
}