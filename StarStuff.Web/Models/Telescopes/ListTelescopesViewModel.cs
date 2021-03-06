﻿namespace StarStuff.Web.Models.Telescopes
{
    using Infrastructure.Helpers;
    using Services.Areas.Moderator.Models.Telescopes;
    using System.Collections.Generic;

    public class ListTelescopesViewModel : BasePageViewModel
    {
        public IEnumerable<ListTelescopesServiceModel> Telescopes { get; set; }
    }
}