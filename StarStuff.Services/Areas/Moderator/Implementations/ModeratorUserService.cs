namespace StarStuff.Services.Areas.Moderator.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Services.Models.Users;
    using System.Collections.Generic;
    using System.Linq;

    public class ModeratorUserService : IModeratorUserService
    {
        private const string Astronomer = "Astronomer";

        private readonly StarStuffDbContext db;

        public ModeratorUserService(StarStuffDbContext db)
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

        public void Approve(int id)
        {
            User user = this.db.Users.Find(id);

            if (user == null)
            {
                return;
            }

            if (user.Roles.Any(r => r.Role.Name == Astronomer))
            {
                return;
            }

            user.SendApplication = false;

            user.Roles.Add(new UserRole
            {
                RoleId = this.db
                    .Roles
                    .FirstOrDefault(r => r.Name == Astronomer)
                    .Id
            });

            this.db.SaveChanges();
        }

        public void Deny(int id)
        {
            User user = this.db.Users.Find(id);

            if (user == null)
            {
                return;
            }

            user.SendApplication = false;

            this.db.SaveChanges();
        }

        public UserDetailsServiceModel ApplicationDetails(int userId)
        {
            return this.db
                .Users
                .Where(u => u.Id == userId)
                .ProjectTo<UserDetailsServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListUsersServiceModel> Applications()
        {
            return this.db
                .Users
                .Where(u => u.SendApplication)
                .OrderBy(u => u.UserName)
                .Take(10)
                .ProjectTo<ListUsersServiceModel>()
                .ToList();
        }
    }
}