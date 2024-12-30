using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.ObjectModel;

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

    public class BirdsRepository : IRepository<Bird>
    {

        private readonly DbContext context;
        private readonly Func<DbContext> createContext;
        private readonly DbSet<Bird> birds;
        public BirdsRepository(DbContext context, Func<DbContext> createContext)
        {
            context.Database.EnsureCreated();
            this.context = context;
            birds = context.Set<Bird>();
            this.createContext = createContext;
        }
        public BirdsRepository(Func<DbContext> createContext)
            : this(createContext(), createContext)
        { }

        public BirdsRepository(string dbSQLiteFullName)
            : this(() => new ApplicationDbContext(dbSQLiteFullName))
        { }

        public async Task<Bird> AddAsync(Bird idDto) => await Task.Run(() =>
        {
            using (DbContext context = createContext())
            {
                DbSet<Bird> birds = context.Set<Bird>();
                birds.Add(idDto);
                context.SaveChanges();
            }
            return birds.Find(idDto.Id) ?? throw new Exception("Не добавилась.");
        });

        public async Task DeleteAsync(int id) => await Task.Run(() =>
        {
            using (DbContext context = createContext())
            {
                DbSet<Bird> birds = context.Set<Bird>();
                Bird brd = birds.Find(id) ?? throw new Exception("Записи с таким Id нет.");
                birds.Remove(brd);
                context.SaveChanges();
            }
            {
                Bird? brd = birds.Find(id);
                if (brd is not null)
                {
                    birds.Entry(brd).Reload();
                }
            }
        });

        public async Task<IEnumerable<Bird>> GetAllAsync() => await Task.Run(() =>
        {
            return GetObservableCollection().ToList().Select(b => b);
        });

        public ObservableCollection<Bird> GetObservableCollection()
        {
            return birds.Local.ToObservableCollection();
        }

        public async Task<Bird> UpdateAsync(Bird idDto) => await Task.Run(() =>
        {
            using (DbContext context = createContext())
            {
                DbSet<Bird> birds = context.Set<Bird>();
                Bird brd = birds.Find(idDto.Id) ?? throw new Exception("Записи с таким Id нет.");

                brd.Name = idDto.Name;
                brd.Description = idDto.Description;
                brd.Arrival = idDto.Arrival;
                brd.Departure = idDto.Departure;
                brd.IsActive = idDto.IsActive;

                birds.Update(brd);
                context.SaveChanges();
            }
            {
                var entity = birds.Local.FindEntry(idDto.Id);
                if (entity is not null)
                    entity.State = EntityState.Detached;
            }

            return birds.Find(idDto.Id)!;
        });

        public async Task LoadAsync() => await Task.Run(birds.Load);
    }
}
