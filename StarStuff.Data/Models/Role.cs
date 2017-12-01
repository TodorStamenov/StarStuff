namespace StarStuff.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class Role : IdentityRole<int>
    {
        public List<UserRole> Users { get; set; } = new List<UserRole>();
    }
}