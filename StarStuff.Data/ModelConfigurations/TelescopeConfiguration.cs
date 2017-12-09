namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TelescopeConfiguration : IEntityTypeConfiguration<Telescope>
    {
        public void Configure(EntityTypeBuilder<Telescope> builder)
        {
            builder
                .HasMany(t => t.Discoveries)
                .WithOne(d => d.Telescope)
                .HasForeignKey(d => d.TelescopeId);

            builder
                .HasIndex(t => t.Name)
                .IsUnique(true);
        }
    }
}