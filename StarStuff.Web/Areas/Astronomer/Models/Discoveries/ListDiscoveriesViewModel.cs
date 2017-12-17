namespace StarStuff.Web.Areas.Astronomer.Models.Discoveries
{
    using Infrastructure.Helpers;
    using Services.Astronomer.Models.Discoveries;
    using System.Collections.Generic;

    public class ListDiscoveriesViewModel : BasePageViewModel
    {
        public bool? Confirmed { get; set; }

        public IEnumerable<ListDiscoveriesServiceModel> Discoveries { get; set; }
    }
}