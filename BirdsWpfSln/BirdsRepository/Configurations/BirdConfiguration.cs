using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdsRepository.Configurations
{
    internal class BirdConfiguration : IEntityTypeConfiguration<Bird>
    {
        public void Configure(EntityTypeBuilder<Bird> builder)
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

            builder.Property(c => c.IsActive);
            builder.Property(c => c.SpecieId)
                .IsRequired(); 
        }
    }
}
