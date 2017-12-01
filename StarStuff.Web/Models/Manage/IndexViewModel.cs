namespace StarStuff.Web.Models.Manage
{
    using Microsoft.AspNetCore.Http;
    using StarStuff.Data;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class IndexViewModel
    {
        public string Username { get; set; }

        public string ProfileImage { get; set; }

        public IFormFile Image { get; set; }

        [Display(Name = "First Name")]
        [StringLength(
            DataConstants.UserConstants.FirstNameMaxLength,
            MinimumLength = DataConstants.UserConstants.FirstNameMinLength)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(
            DataConstants.UserConstants.LastNameMaxLength,
            MinimumLength = DataConstants.UserConstants.LastNameMinLength)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\+\d{12}$", ErrorMessage = "{0} should begin with + sign followed by 12 digits")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}