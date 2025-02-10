using BirdsCommonStandard;
using BirdsRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BirdsRepository
{
    public class BirdsAndSpeciesDbContext(string dbFullName) : DbContext
    {
        public DbSet<Bird>? Birds { get; set; }
        public DbSet<Specie>? Species { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SpeciesConfiguration());
            modelBuilder.ApplyConfiguration(new BirdConfiguration());

            modelBuilder.Entity<Specie>().HasData(HasData.GetSpecies());
            modelBuilder.Entity<Bird>().HasData(HasData.GetBirds());

            base.OnModelCreating(modelBuilder);
        }

        public string DbFullName { get; } = dbFullName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite(DbFullName);

            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlite($"Data Source={DbFullName}");

            optionsBuilder.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]);
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-9OKU3FE\\SQLEXPRESS;Initial Catalog=Birds;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

        }

        private class HasData
        {
            private static readonly Specie[] species = "Воробьи Вороны Ястребы".Split().Select((s, i) => new Specie(i + 1, s)).ToArray();
            private static readonly Bird[] birds =
@"Воробей 1
Дрозд 1
Скворец 1
Снегирь 1
Ворон 2
Сорока 2
Галка 2
Ястреб 3
Орёл 3"
                .Split('\r', '\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(line => line.Split())
                .Select((s, i) => new Bird(i + 1, s[0], "описание для " + s[0], new DateTime(), new DateTime(), true, int.Parse(s[1])))
                .ToArray();
            public static IEnumerable<Specie> GetSpecies() => species.Select(x => x);
            public static IEnumerable<Bird> GetBirds() => birds.Select(x => x);
        }
    }
}
