namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ObserversConfiguration : IEntityTypeConfiguration<Observers>
    {
        public void Configure(EntityTypeBuilder<Observers> builder)
        {
            builder
               .HasKey(o => new { o.ObserverId, o.DiscoveryId });

            builder
                .HasOne(o => o.Observer)
                .WithMany(a => a.Observations)
                .HasForeignKey(o => o.ObserverId);

            builder
                .HasOne(o => o.Discovery)
                .WithMany(a => a.Observers)
                .HasForeignKey(o => o.DiscoveryId);
        }
    }
}