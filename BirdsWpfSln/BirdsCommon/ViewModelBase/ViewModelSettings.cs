using BirdsCommon.Command;

namespace BirdsCommon.ViewModelBase
{
    public class ViewModelSettings(Predicate<RelayCommand>? previewRaiseCanExecuteChanged,
                                   Action<RelayCommand>? afterCreateCommand,
                                   Predicate<BaseInpc>? previewRaisePropertyChanged,
                                   bool isInDesignMode)
    {

        /// <summary>Метод вызываемый перед поднятием события <see cref="RelayCommand.CanExecuteChanged"/>.
        /// Если возвращает <see langword="false"/>, то событие не подымается.
        /// В метод передаётся экземпляр <see cref="RelayCommand"/> для которого надо поднять событие.</summary>
        public Predicate<RelayCommand>? PreviewRaiseCanExecuteChanged { get; } = previewRaiseCanExecuteChanged;

        /// <summary>Метод вызываемой для дополнительной настройки команды, после её создания.
        /// В метод передаётся экземпляр <see cref="RelayCommand"/> который надо настроить.</summary>
        public Action<RelayCommand>? AfterCreateCommand { get; } = afterCreateCommand;

        /// <summary>Метод вызываемый перед поднятием события <see cref="BaseInpc.PropertyChanged"/>.
        /// Если возвращает <see langword="false"/>, то событие не подымается.
        /// В метод передаётся экземпляр <see cref="BaseInpc"/> для которого надо поднять событие.</summary>
        public Predicate<BaseInpc>? PreviewRaisePropertyChanged { get; } = previewRaisePropertyChanged;

        /// <summary>Указывает режим "Разработки".
        /// Нужно в случае, если в рантайм и в разработке VM и Модель должны работать по разному.</summary>
        public bool IsInDesignMode { get; } = isInDesignMode;

        public static ViewModelSettings Empty { get; } = new(null, null, null, false);

        private static ViewModelSettings? def;
        public static ViewModelSettings Default => def ??= Empty;

        public static void SetDefault(ViewModelSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (def is null)
            {
                def = settings;
            }
            else
            {
                throw new Exception("Регистрация Default или обращение к нему уже были.");
            }
        }
    }
}
