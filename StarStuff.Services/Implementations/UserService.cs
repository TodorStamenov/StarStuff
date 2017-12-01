namespace StarStuff.Services.Implementations
{
    using System;
    using StarStuff.Data;
    using StarStuff.Data.Models;

    public class UserService : IUserService
    {
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
    }
}