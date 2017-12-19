﻿namespace StarStuff.Web.Infrastructure.Helpers
{
    public class BasePageViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int FirstPage
        {
            get
            {
                return this.CurrentPage - 10 <= 1 ? 1 : this.CurrentPage - 10;
            }
        }

        public int LastPage
        {
            get
            {
                return this.CurrentPage + 10 >= this.TotalPages ? this.TotalPages : this.CurrentPage + 10;
            }
        }

        public int PrevPage
        {
            get
            {
                return this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;
            }
        }

        public int NextPage
        {
            get
            {
                return this.CurrentPage >= this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
            }
        }
    }
}