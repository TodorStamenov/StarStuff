namespace StarStuff.Services.Infrastructure.Extensions
{
    using Data.Models;
    using System;
    using System.Linq;

    public static class UserExtensions
    {
        public static IQueryable<User> InRole(this IQueryable<User> users, string role)
        {
            if (!string.IsNullOrEmpty(role)
                 && !string.IsNullOrWhiteSpace(role))
            {
                return users
                    .Where(u => u.Roles
                        .Any(r => r.Role.Name.ToLower() == role.ToLower()));
            }

            return users;
        }

        public static IQueryable<User> Filter(this IQueryable<User> users, string search)
        {
            if (!string.IsNullOrEmpty(search)
                && !string.IsNullOrWhiteSpace(search))
            {
                return users
                    .Where(u => u.UserName.ToLower().Contains(search));
            }

            return users;
        }

        public static IQueryable<User> Locked(this IQueryable<User> users, bool locked)
        {
            if (locked)
            {
                return users
                    .Where(u => u.LockoutEnabled
                        && u.LockoutEnd.GetValueOrDefault().DateTime > DateTime.UtcNow);
            }

            return users;
        }
    }
}