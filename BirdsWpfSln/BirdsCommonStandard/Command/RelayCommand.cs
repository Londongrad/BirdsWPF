using System;
using System.Windows.Input;

namespace BirdsCommonStandard
{
    /// <summary>Класс реализующий <see cref="ICommand"/>.<br/>
    /// Реализация взята из <see href="https://www.cyberforum.ru/wpf-silverlight/thread2390714-page4.html#post13535649"/>
    /// и дополнена конструктором для методов без параметра.</summary>
    public class RelayCommand : ICommand
    {
        private readonly CanExecuteHandler<object> canExecute;
        private readonly ExecuteHandler<object> execute;
        private readonly Predicate<RelayCommand> previewRaiseCanExecuteChanged;

        /// <summary>Событие извещающее об изменении состояния команды.</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>Конструктор команды.</summary>
        /// <param name="execute">Исполняющий метод команды.</param>
        /// <param name="canExecute">Метод, возвращающий состояние команды.</param>
        public RelayCommand(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute, Predicate<RelayCommand> previewRaiseCanExecuteChanged = null)
        {
            if (execute is null) throw new ArgumentNullException(nameof(execute));
            if (canExecute is null) throw new ArgumentNullException(nameof(canExecute));
            this.execute = execute;
            this.canExecute = canExecute;
            this.previewRaiseCanExecuteChanged = previewRaiseCanExecuteChanged ?? (_ => true);
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object?}, CanExecuteHandler{object?})"/>
        public RelayCommand(ExecuteHandler<object> execute, Predicate<RelayCommand> previewRaiseCanExecuteChanged = null)
            : this(execute, _ => true, previewRaiseCanExecuteChanged)
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object?}, CanExecuteHandler{object?})"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute, Predicate<RelayCommand> previewRaiseCanExecuteChanged = null)
                : this
                (
                      p => execute(),
                      canExecute is null ? (CanExecuteHandler<object>) null : p => canExecute(),
                      previewRaiseCanExecuteChanged
                )
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object?}, CanExecuteHandler{object?})"/>
        public RelayCommand(ExecuteHandler execute, Predicate<RelayCommand> previewRaiseCanExecuteChanged = null)
                : this
                (
                      p => execute(),
                      previewRaiseCanExecuteChanged
                )
        { }

        /// <summary>Метод, подымающий событие <see cref="CanExecuteChanged"/>.</summary>
        public void RaiseCanExecuteChanged()
        {
            if (previewRaiseCanExecuteChanged(this))
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Вызов метода, возвращающего состояние команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns><see langword="true"/> - если выполнение команды разрешено.</returns>
        public bool CanExecute(object parameter) => canExecute(parameter);

        /// <summary>Вызов выполняющего метода команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter) => execute(parameter);
    }
}
