using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdsRepository.Configurations
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Specie>
    {
        public void Configure(EntityTypeBuilder<Specie> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder
                .HasMany<Bird>()
                .WithOne()
                .HasForeignKey(e => e.SpecieId)
                .IsRequired();
        }
    }
}
