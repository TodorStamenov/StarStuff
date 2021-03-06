﻿namespace StarStuff.Services.Models.Users
{
    using Common.Mapping;
    using Data.Models;

    public class ListUsersServiceModel : IMapFrom<User>
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}