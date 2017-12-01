namespace StarStuff.Web.Areas.Admin.Models
{
    using StarStuff.Services.Admin.Models.Roles;
    using StarStuff.Services.Admin.Models.Users;
    using System.Collections.Generic;

    public class UserRoleEditViewModel
    {
        public UserRolesServiceModel User { get; set; }

        public IEnumerable<RoleServiceModel> Roles { get; set; }
    }
}