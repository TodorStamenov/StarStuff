namespace StarStuff.Services.Models.Users
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;
    using System;
    using AutoMapper;

    public class UserDetailsServiceModel : ListUsersServiceModel, ICustomMapping
    {
        public string ProfileImage { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<User, UserDetailsServiceModel>()
                .ForMember(u => u.ProfileImage, cfg => cfg.MapFrom(u => ServiceConstants.DataImage + Convert.ToBase64String(u.ProfileImage)));
        }
    }
}