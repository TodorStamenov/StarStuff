namespace StarStuff.Services.Admin.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Roles;
    using Models.Users;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Models.Users;
    using System;
    using Infrastructure.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class AdminUserService : IAdminUserService
    {
        private readonly StarStuffDbContext db;

        public AdminUserService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public bool AddToRole(int userId, string roleName)
        {
            var userRoleInfo = this.db
                .Roles
                .Where(r => r.Name == roleName)
                .Select(r => new
                {
                    r.Id,
                    InRole = r.Users.Any(u => u.UserId == userId)
                })
                .FirstOrDefault();

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
            var userRoleInfo = this.db
                .Roles
                .Where(r => r.Name == roleName)
                .Select(r => new
                {
                    r.Id,
                    InRole = r.Users.Any(u => u.UserId == userId)
                })
                .FirstOrDefault();

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

        public int Total(string role, string search)
        {
            return this.db
                .Users
                .Filter(search)
                .InRole(role)
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

        public IEnumerable<RoleServiceModel> AllRoles()
        {
            return this.db
                .Roles
                .ProjectTo<RoleServiceModel>()
                .ToList();
        }

        public IEnumerable<ListUsersServiceModel> All(int page, string role, string search, int usersPerPage)
        {
            return this.db
                .Users
                .Include(u => u.Roles)
                .Filter(search)
                .InRole(role)
                .ProjectTo<ListUsersServiceModel>()
                .OrderBy(u => u.Username)
                .Skip((page - 1) * usersPerPage)
                .Take(usersPerPage)
                .ToList();
        }
    }
}