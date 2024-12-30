using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BirdsCommon
{
    public interface INavigationService : INotifyPropertyChanged
    {
        /// <summary>Текущий выбранный объект. Не важно какой именно.
        /// Реализует INPC или нет, выполянет ли какие-то другие требования - ничто не важно.</summary>
        /// <remarks>При смене значения должно происходить уведомление через интерфейс <see cref="INotifyPropertyChanged"/>.</remarks>

        object? Current { get; }

        /// <summary>Метод переключения текущего объекта.</summary>
        /// <param name="viewModel"></param>
        void NavigateTo(object? viewModel);

        /// <summary>Команда вызывающая метод <see cref="NavigateTo"/> с реализацией по умолчанию.</summary>
        RelayCommand NavigateToCommand => GetCommand();

        private static readonly ConditionalWeakTable<INavigationService, RelayCommand> commands = new();

        private RelayCommand GetCommand()
        {
            if (!commands.TryGetValue(this, out RelayCommand? command))
            {
                command = new RelayCommand(NavigateTo);
                commands.Add(this, command);
            }
            return command;
        }
    }
}
