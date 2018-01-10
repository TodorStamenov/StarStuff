namespace StarStuff.Services.Areas.Admin
{
    using Models.Logs;
    using Models.Roles;
    using Models.Users;
    using Services.Models.Users;
    using System.Collections.Generic;

    public interface IAdminUserService
    {
        string GetUsername(int id);

        void Log(string username, string action, string tableName);

        bool AddToRole(int userId, string roleName);

        bool RemoveFromRole(int userId, string roleName);

        int TotalLogs(string search);

        int TotalUsers(string role, string search, bool locked);

        UserRolesServiceModel Roles(int id);

        IEnumerable<ListLogsServiceModel> Logs(int page, int pageSize, string search);

        IEnumerable<RoleServiceModel> AllRoles();

        IEnumerable<ListUsersServiceModel> All(int page, int pageSize, string role, string search, bool locked);
    }
}