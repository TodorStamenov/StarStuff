namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PioneersConfiguration : IEntityTypeConfiguration<Pioneers>
    {
        public void Configure(EntityTypeBuilder<Pioneers> builder)
        {
            builder
                .HasKey(p => new { p.PioneerId, p.DiscoveryId });

            builder
                .HasOne(p => p.Pioneer)
                .WithMany(a => a.Discoveries)
                .HasForeignKey(p => p.PioneerId);

            builder
                .HasOne(p => p.Discovery)
                .WithMany(a => a.Pioneers)
                .HasForeignKey(p => p.DiscoveryId);
        }
    }
}