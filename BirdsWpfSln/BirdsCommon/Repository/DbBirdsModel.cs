using Microsoft.EntityFrameworkCore;

namespace BirdsCommon.Repository
{
    public class DbBirdsModel : IBirdsModel
    {
        private readonly DbContext context;

        public IRepository<Bird> Birds { get; }
        public IRepository<Specie> Species { get; }

        public Task LoadAsync() => Task.Run(() =>
        {
            context.Set<Bird>().Load();
            context.Set<Specie>().Load();
        });

        /// <summary>Конструктор Репозитория.</summary>
        /// <param name="context">Постоянный Контекст БД используемый для синхронизации через локальный кеш.</param>
        /// <param name="contextCreator">Функция создающая новый одноразовый DbContext из которого можно получить
        /// <see cref="DbSet{TEntity}">DbSet&lt;<typeparamref name="TId"/>&gt;</see>.</param>

        public DbBirdsModel(DbContext context, Func<DbContext> contextCreator)
        {
            context.Database.EnsureCreated();
            this.context = context;

            Birds = new DbRepository<Bird>(context,contextCreator);
            Species = new DbRepository<Specie>(context,contextCreator);
        }

    }
}
