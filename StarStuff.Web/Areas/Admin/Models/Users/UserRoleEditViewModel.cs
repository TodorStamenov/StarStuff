namespace StarStuff.Web.Areas.Admin.Models.Users
{
    using Services.Admin.Models.Roles;
    using Services.Admin.Models.Users;
    using System.Collections.Generic;

    public class UserRoleEditViewModel
    {
        public bool IsUserLocked { get; set; }

        public UserRolesServiceModel User { get; set; }

        public IEnumerable<RoleServiceModel> Roles { get; set; }
    }
}