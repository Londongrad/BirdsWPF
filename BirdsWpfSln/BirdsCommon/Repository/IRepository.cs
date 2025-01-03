using System.Collections.ObjectModel;

namespace BirdsCommon.Repository
{

    /// <summary>Репозиторий <see cref="IdDto"/>.</summary>
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

        ///// <summary>Получение всех записей.</summary>
        ///// <returns>Последовательность <see cref="TId"/>.</returns>
        //Task<IEnumerable<TId>> GetAllAsync();


        /// <summary>Обновление записи Репозитория.</summary>
        /// <param name="idDto">Экземпляр с данными для обновления.
        /// Идентификатор остаётся неизменным, его поменять невозможно.</param>
        /// <remarks>Если не удалось обновить или создать экземпляр, то выкдывается исключение.</remarks>
        Task UpdateAsync(TId idDto);

        ReadOnlyObservableCollection<TId> GetObservableCollection();

        Task LoadAsync();
    }

}
