using BirdsCommon;
using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

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
            // Добавление сущности через другой, одноразовый Контекст БД.
            using (DbContext context = createContext())
            {
                DbSet<Bird> birds = context.Set<Bird>();
                birds.Add(idDto);
                context.SaveChanges();
            }
            // Получение сущности в локальный кеш и её возврат.
            return birds.Find(idDto.Id) ?? throw new Exception("Не добавилась.");
        });

        public async Task DeleteAsync(int id) => await Task.Run(() =>
        {
            // Удаление сущности через другой, одноразовый Контекст БД.
            using (DbContext context = createContext())
            {
                DbSet<Bird> birds = context.Set<Bird>();
                Bird brd = birds.Find(id) ?? throw new Exception("Записи с таким Id нет.");
                birds.Remove(brd);
                context.SaveChanges();
            }
            {
                // Обновление локального кеша, если там есть сущность с таким Id.
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
            birdsReadOnlyObservableCollection ??= new(birds.Local.ToObservableCollection());
            return birdsReadOnlyObservableCollection;
        }

        public async Task UpdateAsync(Bird bird) => await Task.Run(() =>
        {
            // Обновление записи в БД через другой, одноразовый Контекст БД.
            using (DbContext context = createContext()) // Создание одноразового Контекста БД.
            {
                DbSet<Bird> birds = context.Set<Bird>();

                _ = birds.Update(bird);
                _ = context.SaveChanges();
            }
            {
                // Замена сущности в локальном кеше.
                //birds.Local.ToObservableCollection().ReplaceOrAdd(b => b.Id == bird.Id, bird);
                //birds.Local.FindEntry(bird.Id)!.State = EntityState.Unchanged;

                // Обновление сущности с уведомлением привязок через PropertyDescriptor об изменении Bird.IsActive.
                // Вариант показан, как альтернативная реализация.
                // В данной задаче, на практике, более лучшим вариантом будет замена сущности.
                EntityEntry<Bird>? be = birds.Local.FindEntry(bird.Id);
                be!.Reload();
                Bird b = birds.Find(bird.Id) ?? throw new NullReferenceException();
                OnIsActiveChanged(b);
            }
        });

        private static readonly Action<object, EventArgs> isActiveChangedHandler =
            typeof(PropertyDescriptor)
            .GetMethod($"OnValueChanged", BindingFlags.Instance | BindingFlags.NonPublic)!
            .CreateDelegate<Action<object, EventArgs>>(TypeDescriptor.GetProperties(typeof(Bird))[nameof(Bird.IsActive)]);
        private static readonly PropertyChangedEventArgs args = new(string.Empty);
        private static void OnIsActiveChanged(Bird b) => isActiveChangedHandler(b, args);

        public async Task LoadAsync() => await Task.Run(birds.Load);
    }
}
