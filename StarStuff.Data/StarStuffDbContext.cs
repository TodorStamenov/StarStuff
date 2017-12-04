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

        public DbSet<Discovery> Discoveries { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Publication> Publications { get; set; }

        public DbSet<Planet> Planets { get; set; }

        public DbSet<Star> Stars { get; set; }

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

            builder
                .Entity<Pioneers>()
                .HasKey(p => new { p.PioneerId, p.DiscoveryId });

            builder
                .Entity<User>()
                .HasMany(u => u.Discoveries)
                .WithOne(p => p.Pioneer)
                .HasForeignKey(p => p.PioneerId);

            builder
                .Entity<Discovery>()
                .HasMany(d => d.Pioneers)
                .WithOne(p => p.Discovery)
                .HasForeignKey(p => p.DiscoveryId);

            builder
                .Entity<Observers>()
                .HasKey(o => new { o.ObserverId, o.DiscoveryId });

            builder
                .Entity<User>()
                .HasMany(u => u.Observations)
                .WithOne(o => o.Observer)
                .HasForeignKey(o => o.ObserverId);

            builder
                .Entity<Discovery>()
                .HasMany(d => d.Observers)
                .WithOne(o => o.Discovery)
                .HasForeignKey(o => o.DiscoveryId);

            builder
                .Entity<Discovery>()
                .HasMany(d => d.Stars)
                .WithOne(s => s.Discovery)
                .HasForeignKey(s => s.DiscoveryId);

            builder
                .Entity<Discovery>()
                .HasMany(d => d.Planets)
                .WithOne(p => p.Discovery)
                .HasForeignKey(p => p.DiscoveryId);

            builder
                .Entity<Discovery>()
                .HasMany(d => d.Publications)
                .WithOne(p => p.Discovery)
                .HasForeignKey(p => p.DiscoveryId);

            builder
                .Entity<Discovery>()
                .HasIndex(d => d.StarSystem)
                .IsUnique(true);

            builder
                .Entity<Discovery>()
                .Property(d => d.DateMade)
                .HasColumnType("Date");

            builder
                .Entity<Planet>()
                .HasIndex(p => p.Name)
                .IsUnique(true);

            builder
                .Entity<Star>()
                .HasIndex(s => s.Name)
                .IsUnique(true);

            builder
                .Entity<Journal>()
                .HasMany(j => j.Publications)
                .WithOne(p => p.Journal)
                .HasForeignKey(p => p.JournalId);

            builder
                .Entity<Publication>()
                .Property(p => p.ReleaseDate)
                .HasColumnType("Date");
        }
    }
}