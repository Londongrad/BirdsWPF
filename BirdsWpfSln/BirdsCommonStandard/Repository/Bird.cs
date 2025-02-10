using System;

namespace BirdsCommonStandard
{
    /// <summary>Имутабельный класс для Bird (Птица.).</summary>
    public class Bird : IdDto
    {
        /// <summary>Имя.</summary>
        public string Name { get; } 

        /// <summary>Описание.</summary>
        public string Description { get; }

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата. </summary>
        public DateTime Arrival { get; }

        /// <summary>Отправление.</summary>
        public DateTime Departure { get; } 

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get; }

        /// <summary>Внешний ключ.</summary>
        public int SpecieId { get; }

        public Bird(int id,string name, string description, DateTime arrival, DateTime departure, bool isActive, int specieId)
            : base(id)
        {
            Name = name ?? string.Empty;
            Description = description ?? string.Empty;
            Arrival = arrival.Date;
            Departure = departure.Date;
            IsActive = isActive;
            SpecieId = specieId;
        }
    }
}
