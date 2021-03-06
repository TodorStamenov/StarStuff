﻿namespace StarStuff.Services.Areas.Moderator
{
    using Services.Models.Users;
    using System.Collections.Generic;

    public interface IModeratorUserService
    {
        string GetUsername(int id);

        void Approve(int id);

        void Deny(int id);

        UserDetailsServiceModel ApplicationDetails(int userId);

        IEnumerable<ListUsersServiceModel> Applications();
    }
}