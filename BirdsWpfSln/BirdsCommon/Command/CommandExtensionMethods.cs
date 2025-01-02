using System.Windows.Input;

namespace BirdsCommon
{
    public static class CommandExtensionMethods
    {
        public static bool TryExecute(this ICommand command, object? parameter)
        {
            bool can = command.CanExecute(parameter);
            if (can)
                command.Execute(parameter);
            return can;
        }
        public static bool TryExecute(this ICommand command)
          => TryExecute(command, null);
    }

}
