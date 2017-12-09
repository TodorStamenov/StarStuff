namespace StarStuff.Web.Areas.Astronomer.Models.Discoveries
{
    using Services.Astronomer.Models.Astronomers;
    using Services.Astronomer.Models.Discoveries;
    using Services.Astronomer.Models.Planets;
    using Services.Astronomer.Models.Stars;
    using System.Collections.Generic;

    public class DiscoveryDetailsViewModel
    {
        public bool IsPioneer { get; set; }

        public bool IsObserver { get; set; }

        public DiscoveryDetailsServiceModel Discovery { get; set; }

        public IEnumerable<ListStarsServiceModel> Stars { get; set; }

        public IEnumerable<ListPlanetsServiceModel> Planets { get; set; }

        public IEnumerable<AstronomerServiceModel> Pioneers { get; set; }

        public IEnumerable<AstronomerServiceModel> Observers { get; set; }
    }
}