namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class DiscoveryConfiguration : IEntityTypeConfiguration<Discovery>
    {
        public void Configure(EntityTypeBuilder<Discovery> builder)
        {
            builder
                .HasMany(d => d.Stars)
                .WithOne(s => s.Discovery)
                .HasForeignKey(s => s.DiscoveryId);

            builder
                .HasMany(d => d.Planets)
                .WithOne(p => p.Discovery)
                .HasForeignKey(p => p.DiscoveryId);

            builder
                .HasMany(d => d.Publications)
                .WithOne(p => p.Discovery)
                .HasForeignKey(p => p.DiscoveryId);

            builder
                .HasIndex(d => d.StarSystem)
                .IsUnique(true);

            builder
                .Property(d => d.DateMade)
                .HasColumnType("Date");
        }
    }
}