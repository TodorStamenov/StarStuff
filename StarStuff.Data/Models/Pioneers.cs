namespace StarStuff.Data.Models
{
    public class Pioneers
    {
        public int PioneerId { get; set; }

        public User Pioneer { get; set; }

        public int DiscoveryId { get; set; }

        public Discovery Discovery { get; set; }
    }
}