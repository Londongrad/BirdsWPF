using BirdsWPF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdsWPF.Configurations
{
    public class BirdConfiguration : IEntityTypeConfiguration<BirdEntity>
    {
        public void Configure(EntityTypeBuilder<BirdEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Arrival)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Description)
                .HasMaxLength(40);

            builder.Property(c => c.Departure)
                .HasMaxLength(20);
        }
    }
}
