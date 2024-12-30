using System.Collections.ObjectModel;

namespace BirdsCommon.Repository
{

    /// <summary>Репозиторий <see cref="IdDto"/>.</summary>
    public interface IRepository<TId>  where TId : IdDto
    {
        /// <summary>Cоздание и добавление в Репозиторий Bird.</summary>
        /// <param name="idDto">Экземпляр с данными для добавления. Идентификатор <paramref name="id"/> игнорируется.</param>
        /// <returns>Созданный согласно параметров экземляр <see cref="IdDto"/> и добавленный в репозиторий.
        /// Идентификатор, возвращаемего экземпляра, присваивается репозиторием.</returns>
        /// <remarks>Если не удалось создать или добавить экземпляр, то выкдывается исключение.</remarks>
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
        /// <returns>Новый экземляр <see cref="BirdDto"/> с обновлёнными данными.</returns>
        /// <remarks>Если не удалось обновить или создать экземпляр, то выкдывается исключение.</remarks>
        Task<TId> UpdateAsync(TId idDto);

        ObservableCollection<TId> GetObservableCollection();

        Task LoadAsync();
    }

}
