 using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Windows.Media;

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
            //_ = optionsBuilder.UseSqlite(DbFullName);

            //optionsBuilder
            //    .EnableSensitiveDataLogging()
            //    .UseSqlite($"Data Source={DbFullName}");

            //optionsBuilder.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-9OKU3FE\\SQLEXPRESS;Initial Catalog=Birds;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }
    }
}
