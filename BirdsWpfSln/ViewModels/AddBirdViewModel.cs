using BirdsCommon;
using BirdsCommon.Repository;

namespace BirdsViewModels
{
    public class AddBirdViewModel : ViewModelBase
    {
        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get => Get<int>(); set => Set(value); }

        /// <summary>Имя.</summary>
        public string? Name { get => Get<string?>(); set => Set(value); }

        /// <summary>Описание.</summary>
        public string? Description { get => Get<string?>(); set => Set(value); }

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата. <br/>
        /// (Чтобы руками не заполнять поле)</summary>
        public DateOnly Arrival { get => Get<DateOnly>(); set => Set(value); }

        /// <summary>Отправление.</summary>
        public DateOnly Departure { get => Get<DateOnly>(); set => Set(value); }

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get => Get<bool>(); set => Set(value); }

        public AddBirdViewModel()
        {
            Clear();
        }

        public void Clear()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            Arrival = DateOnly.FromDateTime(DateTime.Now);
            Departure = new();
            IsActive = true;
        }
    }
}
