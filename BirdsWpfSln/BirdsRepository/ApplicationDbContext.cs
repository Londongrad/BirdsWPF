 using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Windows.Media;

namespace BirdsRepository
{
    public class ApplicationDbContext(string dbFullName, DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Bird>? Birds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BirdConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public string DbFullName { get; } = dbFullName;
    }
}
