namespace StarStuff.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser<int>
    {
        [MinLength(DataConstants.UserConstants.FirstNameMinLength)]
        [MaxLength(DataConstants.UserConstants.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [MinLength(DataConstants.UserConstants.LastNameMinLength)]
        [MaxLength(DataConstants.UserConstants.LastNameMaxLength)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [MaxLength(DataConstants.UserConstants.MaxImageSize)]
        public byte[] ProfileImage { get; set; }

        public bool SendApplication { get; set; }

        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        public List<Pioneers> Discoveries { get; set; } = new List<Pioneers>();

        public List<Observers> Observations { get; set; } = new List<Observers>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}