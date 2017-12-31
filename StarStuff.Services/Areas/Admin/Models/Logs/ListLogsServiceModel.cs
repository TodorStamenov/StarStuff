﻿namespace StarStuff.Services.Areas.Admin.Models.Logs
{
    using Common.Mapping;
    using Data.Models;
    using System;

    public class ListLogsServiceModel : IMapFrom<Log>
    {
        public string Username { get; set; }

        public string TableName { get; set; }

        public string Action { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}