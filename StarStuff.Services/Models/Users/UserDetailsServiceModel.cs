namespace StarStuff.Services.Models.Users
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using Infrastructure.Extensions;
    using System;

    public class UserDetailsServiceModel : ListUsersServiceModel, ICustomMapping
    {
        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfileImage { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<User, UserDetailsServiceModel>()
                .ForMember(u => u.ProfileImage,
                    cfg => cfg.MapFrom(u => u.ProfileImage.ConvertImage()));
        }
    }
}