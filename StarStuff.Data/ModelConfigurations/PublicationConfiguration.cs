namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
    {
        public void Configure(EntityTypeBuilder<Publication> builder)
        {
            builder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Publication)
                .HasForeignKey(c => c.PublicationId);

            builder
                .Property(p => p.ReleaseDate)
                .HasColumnType("Date");
        }
    }
}