using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

namespace BirdsRepository
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

            builder.HasData(
                new Bird() { Id = 1, Arrival = DateOnly.Parse("19.01.2001", CultureInfo.GetCultureInfo("ru-ru")), Description = "", Name = "Большак 1", IsActive=true },
                new Bird() { Id = 2, Arrival = DateOnly.Parse("19.01.2001", CultureInfo.GetCultureInfo("ru-ru")), Description = "", Name = "Большак 3", IsActive=true  },
                new Bird() { Id = 3, Arrival = DateOnly.Parse("19.01.2001", CultureInfo.GetCultureInfo("ru-ru")), Description = "", Name = "Большак 5", IsActive=true  },
                new Bird() { Id = 4, Arrival = DateOnly.Parse("19.01.2001", CultureInfo.GetCultureInfo("ru-ru")), Description = "", Name = "Большак 78", IsActive=true  },
                new Bird() { Id = 5, Arrival = DateOnly.Parse("19.01.2001", CultureInfo.GetCultureInfo("ru-ru")), Description = "", Name = "Большак 99", IsActive=true  });
        }
    }
}
