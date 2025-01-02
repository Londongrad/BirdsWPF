using BirdsCommon;
using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BirdsRepository
{
    public class DbBirdsRepository : IRepository<Bird>
    {

#pragma warning disable IDE0052 // Удалить непрочитанные закрытые члены
        private readonly DbContext context;
#pragma warning restore IDE0052 // Удалить непрочитанные закрытые члены
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

        private ReadOnlyObservableCollection<Bird>? birdsReadOnlyObservableCollection;
        public ReadOnlyObservableCollection<Bird> GetObservableCollection()
        {
            if (birdsReadOnlyObservableCollection is null)
            {
                birdsReadOnlyObservableCollection = new(birds.Local.ToObservableCollection());
            }
            return birdsReadOnlyObservableCollection;
        }

        public async Task<Bird> UpdateAsync(Bird bird) => await Task.Run(() =>
        {
            // Переменная для новой сущности с обновлёнными данными.
            Bird @new;

            // Обновление записи в БД через другой, одноразовый Контекст БД.
            using (DbContext context = createContext()) // Создание одноразового Контекста БД.
            {
                DbSet<Bird> birds = context.Set<Bird>();

                // Поиск сущности по ключу (Id). Если нет такой сущности, то выкидывание исключения. 
                @new = birds.Find(bird.Id) ?? throw new Exception("Записи с таким Id нет.");

                // Перезапись данных в сущность полученную из нового Контекста.
                birds.Entry(@new).CurrentValues.SetValues(bird);

                //@new.Name = bird.Name;
                //@new.Description = bird.Description;
                //@new.Arrival = bird.Arrival;
                //@new.Departure = bird.Departure;
                //@new.IsActive = false;

                birds.Update(@new);
                context.SaveChanges();
            }
            {
                birds.Local.ToObservableCollection().ReplaceOrAdd(b => b.Id == bird.Id, @new);
                birds.Local.FindEntry(bird.Id)!.State = EntityState.Unchanged;
            }

            return @new;
        });

        public async Task LoadAsync() => await Task.Run(birds.Load);
    }
}
