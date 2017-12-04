namespace StarStuff.Services.Moderator.Models.Telescopes
{
    public class TelescopeDetailsServiceModel : ListTelescopesServiceModel
    {
        public string Location { get; set; }

        public double MirrorDiameter { get; set; }
    }
}