using System;
using System.ComponentModel;

namespace BirdsCommonStandard
{
    public interface INavigationService : INotifyPropertyChanged
    {
        /// <summary>Текущий выбранный объект. Не важно какой именно.
        /// Реализует INPC или нет, выполянет ли какие-то другие требования - ничто не важно.</summary>
        /// <remarks>При смене значения должно происходить уведомление через интерфейс <see cref="INotifyPropertyChanged"/>.</remarks>

        object Current { get; }

        /// <summary>Метод переключения текущего объекта.</summary>
        /// <param name="viewModel"></param>
        void NavigateTo(object viewModel);

        /// <summary>Команда вызывающая метод <see cref="NavigateTo"/> с реализацией по умолчанию.</summary>
        RelayCommand NavigateToCommand { get; }
        RelayCommand<Type> NavigateToTypeCommand { get; }
        void NavigateToType(Type type);
        void AddCreator(Type type, Func<object> creator);
    }
}
