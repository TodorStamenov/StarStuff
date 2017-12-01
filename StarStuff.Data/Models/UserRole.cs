namespace StarStuff.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}