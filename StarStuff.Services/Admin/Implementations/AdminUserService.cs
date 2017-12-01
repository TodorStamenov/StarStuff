namespace StarStuff.Services.Admin.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Roles;
    using Models.Users;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Models.Users;
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

        public bool AddToRole(int userId, string roleName)
        {
            var userRoleInfo = this.db
                .Roles
                .Where(r => r.Name == roleName)
                .Select(r => new
                {
                    Id = r.Id,
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
                    Id = r.Id,
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
            IQueryable<User> usersQuery = this.Users();

            if (role != null)
            {
                usersQuery = this.RolesQuery(role, usersQuery);
            }

            if (search != null)
            {
                search = search.ToLower();
                usersQuery = this.SearchUsers(search, usersQuery);
            }

            return usersQuery.Count();
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
            IQueryable<User> usersQuery = this.Users();

            if (role != null)
            {
                usersQuery = this.RolesQuery(role, usersQuery);
            }

            if (search != null)
            {
                usersQuery = this.SearchUsers(search, usersQuery);
            }

            return usersQuery
                .OrderBy(u => u.UserName)
                .Skip((page - 1) * usersPerPage)
                .Take(usersPerPage)
                .ProjectTo<ListUsersServiceModel>()
                .ToList();
        }

        public IQueryable<User> Users()
        {
            return this.db.Users.AsQueryable();
        }

        private IQueryable<User> RolesQuery(string role, IQueryable<User> usersQuery)
        {
            return usersQuery
                .Where(u => u.Roles.Any(r => r.Role.Name == role));
        }

        private IQueryable<User> SearchUsers(string search, IQueryable<User> usersQuery)
        {
            return usersQuery
                    .Where(u => u.UserName.ToLower().Contains(search)
                    || u.FirstName.ToLower().Contains(search)
                    || u.LastName.ToLower().Contains(search)
                    || u.Email.ToLower().Contains(search));
        }
    }
}