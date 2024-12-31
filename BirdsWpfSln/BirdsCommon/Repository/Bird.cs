namespace BirdsCommon.Repository
{
    /// <summary>Имутабельный класс для Bird (Птицы ???).</summary>
    public record Bird : IdDto
    {
        /// <summary>Имя.</summary>
        public string? Name { get; set; }

        /// <summary>Описание.</summary>
        public string? Description { get; set; }

        /// <summary>Прибытие ???</summary>
        public DateOnly Arrival { get; set; }

        /// <summary>Отправление ???</summary>
        public DateOnly Departure { get; set; }

        /// <summary>Активная Птица ???</summary>
        public bool IsActive { get; set; }
    }
}
