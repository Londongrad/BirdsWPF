using BirdsWPF.Configurations;
using BirdsWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace BirdsWPF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BirdEntity>? Birds { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) => Database.EnsureCreated();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BirdConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
