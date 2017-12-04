namespace StarStuff.Services.Admin.Models.Users
{
    using AutoMapper;
    using Roles;
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserRolesServiceModel : IMapFrom<User>, ICustomMapping
    {
        public int Id { get; set; }

        public string ProfileImage { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<RoleServiceModel> Roles { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<User, UserRolesServiceModel>()
                .ForMember(u => u.Roles,
                    cfg => cfg.MapFrom(
                        u => u.Roles.Select(r => new RoleServiceModel { Name = r.Role.Name })))
                .ForMember(u => u.ProfileImage,
                    cfg => cfg.MapFrom(
                        u => ServiceConstants.DataImage + Convert.ToBase64String(u.ProfileImage)));
        }
    }
}