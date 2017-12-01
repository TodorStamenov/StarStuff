namespace StarStuff.Web.Areas.Admin.Models
{
    using StarStuff.Services.Admin.Models.Roles;
    using StarStuff.Services.Models.Users;
    using System.Collections.Generic;

    public class UserListViewModel
    {
        public string Search { get; set; }

        public string UserRole { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

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

        public IEnumerable<ListUsersServiceModel> Users { get; set; }

        public IEnumerable<RoleServiceModel> Roles { get; set; }
    }
}