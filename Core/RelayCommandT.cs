namespace BirdsWPF.Core
{
    /// <summary>Реализация RelayCommand для методов с обобщённым параметром.</summary>
    /// <typeparam name="T">Тип параметра методов.</typeparam>
    public class RelayCommand<T> : RelayCommand
    {
        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler<T> execute, CanExecuteHandler<T>? canExecute = null)
            : base
            (
                  p =>
                  {
                      if (p is T t)
                          execute(t);
                  },
              canExecute is null ? null : p => (p is T t) && (canExecute(t))
            )
        { }
    }
}
