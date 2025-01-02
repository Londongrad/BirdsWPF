namespace BirdsCommon.Repository
{
    /// <summary>Имутабельный класс для Bird (Птица.).</summary>
    public record Bird : IdDto
    {
        /// <summary>Имя.</summary>
        public string? Name { get; set; }

        /// <summary>Описание.</summary>
        public string? Description { get; set; }

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата. <br/>
        /// (Чтобы руками не заполнять поле)</summary>
        public DateOnly Arrival { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        /// <summary>Отправление.</summary>
        public DateOnly Departure { get; set; }

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get; set; } = true;
    }
}
