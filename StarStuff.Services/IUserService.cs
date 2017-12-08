namespace StarStuff.Services
{
    using Models.Users;
    using System;
    using System.Collections.Generic;

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

        int TotalAstronomers();

        IEnumerable<ListAstronomersServiceModel> Astronomers(int page, int pageSize);
    }
}