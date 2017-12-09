﻿namespace StarStuff.Web.Models.Journals
{
    using Infrastructure;
    using Services.Moderator.Models.Journals;
    using System.Collections.Generic;

    public class ListJournalsViewModel : BasePageViewModel
    {
        public IEnumerable<ListJournalsServiceModel> Journals { get; set; }
    }
}