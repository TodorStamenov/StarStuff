namespace StarStuff.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Discovery
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.DiscoveryConstants.StarSystemMaxLength)]
        public string StarSystem { get; set; }

        public DateTime DateMade { get; set; }

        public int TelescopeId { get; set; }

        public Telescope Telescope { get; set; }

        public List<Planet> Planets { get; set; } = new List<Planet>();

        public List<Star> Stars { get; set; } = new List<Star>();

        public List<Publication> Publications { get; set; } = new List<Publication>();

        public List<Pioneers> Pioneers { get; set; } = new List<Pioneers>();

        public List<Observers> Observers { get; set; } = new List<Observers>();
    }
}