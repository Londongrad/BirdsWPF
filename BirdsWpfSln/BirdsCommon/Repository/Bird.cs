using System.Diagnostics.CodeAnalysis;

namespace BirdsCommon.Repository
{
    /// <summary>Имутабельный класс для Bird (Птица.).</summary>
    public class Bird(int id, string? name, string? description, DateOnly arrival, DateOnly departure, bool isActive) : IdDto(id)
    {

        /// <summary>Имя.</summary>
        public string? Name { get; } = name;

        /// <summary>Описание.</summary>
        public string? Description { get; } = description;

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата. <br/>
        /// (Чтобы руками не заполнять поле)</summary>
        public DateOnly Arrival { get; } = arrival;

        /// <summary>Отправление.</summary>
        public DateOnly Departure { get; } = departure;

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get; } = isActive;

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ string.GetHashCode(Name);
        }

        public override bool ValueEquals(IdDto? other)
        {
            if (!base.ValueEquals(other))
                return false;

            Bird bird = (Bird)other!;

            return string.Equals(Name, bird.Name) &&
                string.Equals(Description, bird.Description) &&
                Arrival == bird.Arrival &&
                Departure == bird.Departure &&
                IsActive == bird.IsActive;
        }
    }
}
