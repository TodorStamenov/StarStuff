namespace StarStuff.Services
{
    using Models.Astronomers;
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

        IEnumerable<AstronmersServiceModel> Astronomers(int astronomerId);

        IEnumerable<ListAstronomersServiceModel> Astronomers(int page, int pageSize);
    }
}