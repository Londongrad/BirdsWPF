using BirdsCommonStandard;
using System;

namespace BirdsViewModels
{
    public class AddBirdViewModel : ViewModelBase
    {
        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get => Get<int>(); set => Set(value); }

        /// <summary>Имя.</summary>
        public string Name { get => Get<string>(); set => Set(value); }

        /// <summary>Описание.</summary>
        public string Description { get => Get<string>(); set => Set(value); }

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата. <br/>
        /// (Чтобы руками не заполнять поле)</summary>
        public DateTime Arrival { get => Get<DateTime>(); set => Set(value); }

        /// <summary>Отправление.</summary>
        public DateTime Departure { get => Get<DateTime>(); set => Set(value); }

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get => Get<bool>(); set => Set(value); }

        /// <summary>Целочисленный идентификатор вида.</summary>
        public int SpecieId { get => Get<int>(); set => Set(value); }

        public AddBirdViewModel(ViewModelSettings settings) : base(settings)
        {
            Clear();
        }

        public void Clear()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            Arrival = DateTime.Now;
            Departure = new DateTime();
            IsActive = true;
            SpecieId = 0;
        }
    }
}
