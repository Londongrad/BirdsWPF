using BirdsCommon.Repository;
using BirdsRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BirdsRepository
{
    public partial class ApplicationDbContext(string dbFullName) : DbContext
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
        }

    }
}
