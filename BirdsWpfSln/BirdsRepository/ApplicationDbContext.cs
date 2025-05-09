using BirdsCommon.Repository;
using BirdsRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BirdsRepository
{
    internal class ApplicationDbContext(string dbFullName) : DbContext
    {
        public DbSet<Bird>? Birds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BirdConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public string DbFullName { get; } = dbFullName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .EnableSensitiveDataLogging()
            //    .UseSqlite($"Data Source={DbFullName}");

            //optionsBuilder.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-B2DG91S\\SQLEXPRESS;Initial Catalog=Birds;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
