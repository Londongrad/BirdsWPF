using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace BirdsCommon.Repository
{

    /// <summary>Безопасный Репозиторий.<see cref="IdDto"/>.</summary>
    public interface IRepository<TId> where TId : IdDto
    {
        /// <summary>Добавление записи в Репозиторий.</summary>
        /// <param name="idDto">Экземпляр с данными для добавления. Идентификатор <paramref name="id"/> игнорируется.</param>
        /// <remarks>Если не удалось создать или добавить экземпляр, то выкдывается исключение.
        /// Игнорирование <see cref="TId.Id"/> зависит от диалекта БД.
        /// Возможно для этого добавление логики в Репозиторий.</remarks>
        /// <returns>Созданный согласно параметров экземляр <see cref="IdDto"/> и добавленный в репозиторий.
        /// Идентификатор, возвращаемего экземпляра, присваивается репозиторием.</returns>

        Task<TId> AddAsync(TId idDto);

        /// <summary>Удаление записи с указанным идентификатором.</summary>
        /// <param name="id">Идентификатор.</param>
        /// <remarks>Если не удалось удалить, то выкидывается исключение.</remarks>
        Task DeleteAsync(int id);

        /// <summary>Получение всех записей.</summary>
        /// <returns>Последовательность <see cref="TId"/>.</returns>
        Task<IEnumerable<TId>> GetAllAsync();


        /// <summary>Обновление записи Репозитория.</summary>
        /// <param name="idDto">Экземпляр с данными для обновления.
        /// Идентификатор остаётся неизменным, его поменять невозможно.</param>
        /// <returns>Новая сущность с обновлёнными свойствами.</returns>
        /// <remarks>Если не удалось обновить или создать экземпляр, то выкдывается исключение.</remarks>
        Task<TId> UpdateAsync(TId idDto);

        /// <summary>Событие уведомляющее об изменении Репозитория.</summary>
        event EventHandler<RepChangedArgs<TId>>? RepChanged;
    }

    public class Repository<TId> : IRepository<TId> where TId : IdDto
    {
        private readonly Func<DbContext> contextCreator;

        /// <summary>Конструктор Репозитория.</summary>
        /// <param name="contextCreator">Функция создающая новый одноразовый DbContext из которого можно получить <see cref="DbSet{TEntity}">DbSet&lt;<typeparamref name="TId"/>&gt;</see>.</param>
        public Repository(Func<DbContext> contextCreator)
        {
            ArgumentNullException.ThrowIfNull(contextCreator);
            this.contextCreator = contextCreator;

            // Проверка наличия БД и нужного DbSet.
            DbContext context = contextCreator();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            _ = context.Set<TId>() ?? throw new ArgumentException($"{nameof(contextCreator)} создаёт {nameof(DbContext)} в котором нет нужного {nameof(DbSet<TId>)}.");
        }

        public event EventHandler<RepChangedArgs<TId>>? RepChanged;

        public async Task<TId> AddAsync(TId idDto)
        {
            await Task.Run(() =>
            {
                using (DbContext context = contextCreator())
                {
                    DbSet<TId> set = context.Set<TId>();
                    set.Add(idDto);
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                    idDto = set.Find(idDto.Id) ?? throw new Exception("Чё-то не так.");
                }
            });
            RepChanged?.Invoke(this, RepChangedArgs<TId>.Add(idDto));
            return idDto;
        }

        public async Task DeleteAsync(int id)
        {
            TId idDto = await Task.Run(() =>
                        {
                            using (DbContext context = contextCreator())
                            {
                                DbSet<TId> set = context.Set<TId>();
                                TId idDto = set.Find(id) ?? throw new Exception($"Нет сущности с {nameof(id)}={id}.");
                                set.Remove(idDto);
                                context.SaveChanges();
                                return idDto;
                            }
                        });
            RepChanged?.Invoke(this, RepChangedArgs<TId>.Remove(idDto));
        }

        public async Task<IEnumerable<TId>> GetAllAsync()
        {
            ImmutableArray<TId> array = await Task.Run(() =>
            {
                using (DbContext context = contextCreator())
                {
                    DbSet<TId> set = context.Set<TId>();
                    return set.ToImmutableArray();
                }
            });

            return array;
        }

        public async Task<TId> UpdateAsync(TId idDto)
        {
            TId old = await Task.Run(() =>
                      {
                          using (DbContext context = contextCreator())
                          {
                              DbSet<TId> set = context.Set<TId>();
                              TId old = set.Find(idDto.Id) ?? throw new Exception($"Нет сущности с {nameof(idDto.Id)}={idDto.Id}.");
                              context.ChangeTracker.Clear();
                              set.Update(idDto);
                              context.SaveChanges();
                              context.ChangeTracker.Clear();
                              idDto = set.Find(idDto.Id) ?? throw new Exception("Чё-то не так.");
                              return old;
                          }
                      });
            RepChanged?.Invoke(this, RepChangedArgs<TId>.Update(old, idDto));
            return idDto;
        }
    }
}
