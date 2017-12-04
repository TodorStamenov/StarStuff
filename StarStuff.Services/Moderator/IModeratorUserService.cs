namespace StarStuff.Services.Moderator
{
    using StarStuff.Services.Models.Users;
    using System.Collections.Generic;

    public interface IModeratorUserService
    {
        void Approve(int id);

        void Deny(int id);

        UserDetailsServiceModel ApplicationDetails(int userId);

        IEnumerable<ListUsersServiceModel> Applications();
    }
}