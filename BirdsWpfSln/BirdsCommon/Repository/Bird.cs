namespace BirdsCommon.Repository
{
    /// <summary>Имутабельный класс для Bird (Птица.).</summary>
    public class Bird(int id, string name, string? description, DateOnly arrival, DateOnly departure, bool isActive, int specieId)
        : IdDto(id)
    {
        /// <summary>Имя.</summary>
        public string Name { get; } = name;

        /// <summary>Описание.</summary>
        public string? Description { get; } = description;

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата. </summary>
        public DateOnly Arrival { get; } = arrival;

        /// <summary>Отправление.</summary>
        public DateOnly Departure { get; } = departure;

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get; } = isActive;

        /// <summary>Внешний ключ.</summary>
        public int SpecieId { get; } = specieId;

    }
}
