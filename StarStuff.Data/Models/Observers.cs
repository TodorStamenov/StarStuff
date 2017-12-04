namespace StarStuff.Data.Models
{
    public class Observers
    {
        public int ObserverId { get; set; }

        public User Observer { get; set; }

        public int DiscoveryId { get; set; }

        public Discovery Discovery { get; set; }
    }
}