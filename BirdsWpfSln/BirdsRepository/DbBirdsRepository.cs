using BirdsCommon;
using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BirdsRepository
{
    public class DbBirdsRepository : IRepository<Bird>
    {

        private readonly DbContext context;
        private readonly Func<DbContext> createContext;
        private readonly DbSet<Bird> birds;
        public DbBirdsRepository(DbContext context, Func<DbContext> createContext)
        {
            context.Database.EnsureCreated();
            this.context = context;
            birds = context.Set<Bird>();
            this.createContext = createContext;
        }
        public DbBirdsRepository(Func<DbContext> createContext)
            : this(createContext(), createContext)
        { }

        public DbBirdsRepository(string dbSQLiteFullName)
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

        public async Task<Bird> UpdateAsync(Bird bird) => await Task.Run(() =>
        {
            Bird @new;
            using (DbContext context = createContext())
            {
                DbSet<Bird> birds = context.Set<Bird>();
                @new = birds.Find(bird.Id) ?? throw new Exception("Записи с таким Id нет.");

                @new.Name = bird.Name;
                @new.Description = bird.Description;
                @new.Arrival = bird.Arrival;
                @new.Departure = bird.Departure;
                @new.IsActive = false;

                birds.Update(@new);
                context.SaveChanges();
            }
            {
                GetObservableCollection().ReplaceOrAdd(b => b.Id == bird.Id, @new);
                birds.Local.FindEntry(bird.Id)!.State = EntityState.Unchanged;
            }

            return @new;
        });

        public async Task LoadAsync() => await Task.Run(birds.Load);
    }
}
