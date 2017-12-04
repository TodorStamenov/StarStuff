namespace StarStuff.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StarStuffDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public StarStuffDbContext(DbContextOptions<StarStuffDbContext> options)
            : base(options)
        {
        }

        public DbSet<Telescope> Telescopes { get; set; }

        public DbSet<Journal> Journals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<UserRole>()
                .HasOne(e => e.User)
                .WithMany(e => e.Roles)
                .HasForeignKey(e => e.UserId);

            builder
                .Entity<UserRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);

            builder
                .Entity<User>()
                .Property(u => u.BirthDate)
                .HasColumnType("Date");
        }
    }
}