using BirdsCommon.Repository;
using BirdsViewModels;
using System.Windows.Markup;

namespace BirdsWPF.Views.UserControls
{

    [MarkupExtensionReturnType(typeof(BirdVM))]
    public class BirdExtension : MarkupExtension
    {
        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get; set; }

        /// <summary>Имя.</summary>
        public string? Name { get; set; }

        /// <summary>Описание.</summary>
        public string? Description { get; set; }

        /// <summary>Прибытие. Для простоты ввода по умолчанию установлена текущая дата.</summary>
        public DateOnly Arrival { get; set; }

        /// <summary>Отправление.</summary>
        public DateOnly Departure { get; set; }

        /// <summary>Активная Птица.</summary>
        public bool IsActive { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BirdVM(new Bird(Id, Name, Description, Arrival, Departure, IsActive));
        }
    }
}
