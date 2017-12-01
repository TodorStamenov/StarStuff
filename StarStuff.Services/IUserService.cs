using System;

namespace StarStuff.Services
{
    public interface IUserService
    {
        bool Apply(int id);

        bool AddProfileImage(int userId, byte[] imageContent);

        bool UpdateUser(
            int userId,
            string firstName,
            string lastName,
            string email,
            DateTime? birthDate,
            string phoneNumber);
    }
}