namespace StarStuff.Data.ModelConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class JournalConfiguration : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder
                .HasMany(j => j.Publications)
                .WithOne(p => p.Journal)
                .HasForeignKey(p => p.JournalId);
        }
    }
}