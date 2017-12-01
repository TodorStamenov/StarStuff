namespace StarStuff.Services.Admin
{
    using Models.Roles;
    using Models.Users;
    using StarStuff.Services.Models.Users;
    using System.Collections.Generic;

    public interface IAdminUserService
    {
        bool AddToRole(int userId, string roleName);

        bool RemoveFromRole(int userId, string roleName);

        int Total(string role, string search);

        UserRolesServiceModel Roles(int id);

        IEnumerable<ListUsersServiceModel> All(int page, string role, string search, int usersPerPage);

        IEnumerable<RoleServiceModel> AllRoles();
    }
}