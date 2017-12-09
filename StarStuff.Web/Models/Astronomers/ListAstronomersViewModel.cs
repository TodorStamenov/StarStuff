namespace StarStuff.Web.Models.Astronomers
{
    using Infrastructure;
    using Services.Models.Astronomers;
    using System.Collections.Generic;

    public class ListAstronomersViewModel : BasePageViewModel
    {
        public IEnumerable<ListAstronomersServiceModel> Astronomers { get; set; }
    }
}