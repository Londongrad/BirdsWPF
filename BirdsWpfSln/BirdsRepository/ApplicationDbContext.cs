using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BirdsRepository
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Bird>? Birds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BirdConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public string DbFullName { get; }

        public ApplicationDbContext(string dbFullName)
        {
            DbFullName = dbFullName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite(DbFullName);

            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlite($"Data Source={DbFullName}");

            optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });


        }

    }
}
