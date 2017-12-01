﻿namespace StarStuff.Services.Models.Users
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class ListUsersServiceModel : IMapFrom<User>
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}