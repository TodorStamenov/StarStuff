namespace StarStuff.Web.Models.Astronomers
{
    using StarStuff.Services.Models.Users;
    using System.Collections.Generic;

    public class ListAstronomersViewModel : BasePageViewModel
    {
        public IEnumerable<ListAstronomersServiceModel> Astronomers { get; set; }
    }
}