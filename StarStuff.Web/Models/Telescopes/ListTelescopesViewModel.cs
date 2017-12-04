namespace StarStuff.Web.Models.Telescopes
{
    using StarStuff.Services.Moderator.Models.Telescopes;
    using System.Collections.Generic;

    public class ListTelescopesViewModel : BasePageViewModel
    {
        public IEnumerable<ListTelescopesServiceModel> Telescopes { get; set; }
    }
}