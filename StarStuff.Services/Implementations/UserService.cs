namespace StarStuff.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private const string Astronomer = "Astronomer";

        private readonly StarStuffDbContext db;

        public UserService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public bool Apply(int id)
        {
            User user = this.db.Users.Find(id);

            if (user == null)
            {
                return false;
            }

            user.SendApplication = true;

            this.db.SaveChanges();

            return true;
        }

        public bool AddProfileImage(int userId, byte[] imageContent)
        {
            User user = this.db.Users.Find(userId);

            if (user == null)
            {
                return false;
            }

            user.ProfileImage = imageContent;

            this.db.SaveChanges();

            return true;
        }

        public bool UpdateUser(
            int userId,
            string firstName,
            string lastName,
            string email,
            DateTime? birthDate,
            string phoneNumber)
        {
            User user = this.db.Users.Find(userId);

            if (user == null)
            {
                return false;
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.BirthDate = birthDate;
            user.PhoneNumber = phoneNumber;

            this.db.SaveChanges();

            return true;
        }

        public int TotalAstronomers()
        {
            return this.db
                .Users
                .Count(u => u.Roles.Any(r => r.Role.Name == Astronomer));
        }

        public IEnumerable<ListAstronomersServiceModel> Astronomers(int page, int pageSize)
        {
            return this.db
                .Users
                .Where(u => u.Roles.Any(r => r.Role.Name == Astronomer))
                .OrderByDescending(a => a.Discoveries.Count)
                .ThenByDescending(a => a.Observations.Count)
                .ThenBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListAstronomersServiceModel>()
                .ToList();
        }
    }
}