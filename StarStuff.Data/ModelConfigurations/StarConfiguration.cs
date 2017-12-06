namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class StarConfiguration : IEntityTypeConfiguration<Star>
    {
        public void Configure(EntityTypeBuilder<Star> builder)
        {
            builder
                .HasIndex(s => s.Name)
                .IsUnique(true);
        }
    }
}