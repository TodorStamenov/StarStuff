namespace StarStuff.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using ModelConfigurations;
    using Models;

    public class StarStuffDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public StarStuffDbContext(DbContextOptions<StarStuffDbContext> options)
            : base(options)
        {
        }

        public DbSet<Telescope> Telescopes { get; set; }

        public DbSet<Discovery> Discoveries { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Publication> Publications { get; set; }

        public DbSet<Planet> Planets { get; set; }

        public DbSet<Star> Stars { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new PioneersConfiguration());
            builder.ApplyConfiguration(new ObserversConfiguration());
            builder.ApplyConfiguration(new DiscoveryConfiguration());
            builder.ApplyConfiguration(new PublicationConfiguration());
            builder.ApplyConfiguration(new JournalConfiguration());
            builder.ApplyConfiguration(new StarConfiguration());
            builder.ApplyConfiguration(new PlanetConfiguration());
            builder.ApplyConfiguration(new TelescopeConfiguration());
        }
    }
}