using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace BirdsCommon.Repository
{
    public class Repository<TId> : IRepository<TId> where TId : IdDto
    {
        private readonly Func<DbContext> contextCreator;

        /// <summary>Конструктор Репозитория.</summary>
        /// <param name="contextCreator">Функция создающая новый одноразовый DbContext из которого можно получить <see cref="DbSet{TEntity}">DbSet&lt;<typeparamref name="TId"/>&gt;</see>.</param>
        public Repository(Func<DbContext> contextCreator)
        {
            if (contextCreator is null)
            {
                throw new ArgumentNullException(nameof(contextCreator));
            }

            this.contextCreator = contextCreator;

            // Проверка наличия БД и нужного DbSet.
            using (DbContext context = contextCreator())
            {
                context.Database.EnsureCreated();
                _ = context.Set<TId>() ?? throw new ArgumentException($"{nameof(contextCreator)} создаёт {nameof(DbContext)} в котором нет нужного {nameof(DbSet<TId>)}.");
            }
        }

        public event EventHandler<RepChangedArgs<TId>> RepChanged;

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
                    TId idDto1 = set.Find(id) ?? throw new Exception($"Нет сущности с {nameof(id)}={id}.");
                    set.Remove(idDto1);
                    context.SaveChanges();
                    return idDto1;
                }
            });
            RepChanged?.Invoke(this, RepChangedArgs<TId>.Remove(idDto));
        }

        public async Task<IEnumerable<TId>> GetAllAsync()
        {
            TId[] array = await Task.Run(() =>
            {
                using (DbContext context = contextCreator())
                {
                    DbSet<TId> set = context.Set<TId>();
                    return set.ToArray();
                }
            });

            return Array.AsReadOnly(array);
        }

        public async Task<TId> UpdateAsync(TId idDto)
        {
            TId old = await Task.Run(() =>
            {
                using (DbContext context = contextCreator())
                {
                    DbSet<TId> set = context.Set<TId>();
                    TId old1 = set.Find(idDto.Id) ?? throw new Exception($"Нет сущности с {nameof(idDto.Id)}={idDto.Id}.");
                    context.ChangeTracker.Clear();
                    set.Update(idDto);
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                    idDto = set.Find(idDto.Id) ?? throw new Exception("Чё-то не так.");
                    return old1;
                }
            });
            RepChanged?.Invoke(this, RepChangedArgs<TId>.Update(old, idDto));
            return idDto;
        }
    }

}
