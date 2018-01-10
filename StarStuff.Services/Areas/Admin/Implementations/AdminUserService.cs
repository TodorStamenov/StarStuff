namespace StarStuff.Services.Areas.Admin.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models.Logs;
    using Models.Roles;
    using Models.Users;
    using Services.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminUserService : IAdminUserService
    {
        private readonly StarStuffDbContext db;

        public AdminUserService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public string GetUsername(int id)
        {
            return this.db
                .Users
                .Where(u => u.Id == id)
                .Select(u => u.UserName)
                .FirstOrDefault();
        }

        public void Log(string username, string action, string tableName)
        {
            Log log = new Log
            {
                Username = username,
                Action = action,
                TableName = tableName,
                TimeStamp = DateTime.UtcNow
            };

            this.db.Logs.Add(log);
            this.db.SaveChanges();
        }

        public bool AddToRole(int userId, string roleName)
        {
            UserRoleInfoServiceModel userRoleInfo = this.GetUserRoleInfo(userId, roleName);

            if (userRoleInfo == null
                || userRoleInfo.InRole)
            {
                return false;
            }

            UserRole userRole = new UserRole
            {
                RoleId = userRoleInfo.Id,
                UserId = userId
            };

            this.db.Add(userRole);
            this.db.SaveChanges();

            return true;
        }

        public bool RemoveFromRole(int userId, string roleName)
        {
            UserRoleInfoServiceModel userRoleInfo = this.GetUserRoleInfo(userId, roleName);

            if (userRoleInfo == null
                || !userRoleInfo.InRole)
            {
                return false;
            }

            UserRole userRole = this.db
                .Find<UserRole>(userId, userRoleInfo.Id);

            this.db.Remove(userRole);
            this.db.SaveChanges();

            return true;
        }

        public int TotalLogs(string search)
        {
            return this.db
                .Logs
                .Filter(search)
                .Count();
        }

        public int TotalUsers(string role, string search, bool locked)
        {
            return this.db
                .Users
                .Filter(search)
                .InRole(role)
                .Locked(locked)
                .Count();
        }

        public UserRolesServiceModel Roles(int id)
        {
            return this.db
                .Users
                .Where(u => u.Id == id)
                .ProjectTo<UserRolesServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListLogsServiceModel> Logs(int page, int itemsPerPage, string search)
        {
            return this.db
                .Logs
                .Filter(search)
                .OrderByDescending(l => l.TimeStamp)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ProjectTo<ListLogsServiceModel>()
                .ToList();
        }

        public IEnumerable<RoleServiceModel> AllRoles()
        {
            return this.db
                .Roles
                .ProjectTo<RoleServiceModel>()
                .ToList();
        }

        public IEnumerable<ListUsersServiceModel> All(int page, int pageSize, string role, string search, bool locked)
        {
            return this.db
                .Users
                .Include(u => u.Roles)
                .Filter(search)
                .InRole(role)
                .Locked(locked)
                .ProjectTo<ListUsersServiceModel>()
                .OrderBy(u => u.Username)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        private UserRoleInfoServiceModel GetUserRoleInfo(int userId, string roleName)
        {
            return this.db
                .Roles
                .Where(r => r.Name == roleName)
                .Select(r => new UserRoleInfoServiceModel
                {
                    Id = r.Id,
                    InRole = r.Users.Any(u => u.UserId == userId)
                })
                .FirstOrDefault();
        }
    }
}