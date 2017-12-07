namespace StarStuff.Web.Areas.Astronomer.Models.Discoveries
{
    using StarStuff.Services.Astronomer.Models.Discoveries;
    using StarStuff.Web.Models;
    using System.Collections.Generic;

    public class ListDiscoveriesViewModel : BasePageViewModel
    {
        public bool? Confirmed { get; set; }

        public IEnumerable<ListDiscoveriesServiceModel> Discoveries { get; set; }
    }
}