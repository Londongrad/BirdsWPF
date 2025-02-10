using System;

namespace BirdsCommonStandard
{
    /// <summary>Базовый класс для ViewModel.</summary>
    public abstract partial class ViewModelBase : BaseInpc
    {
        private readonly ViewModelSettings settings;

        protected ViewModelBase(ViewModelSettings settings)
            : base(settings.PreviewRaisePropertyChanged)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected ViewModelBase()
            : this(ViewModelSettings.Default)
        { }

        /// <summaryВозвращает <see langword="true"/>, если обращение происходит
        /// во Время режима Разработки.<br/>
        /// Инициализируется значение свойства <see cref="IsInDesignModeStatic"/>.<br/>
        /// В производных классах может быть, через конструкторы
        /// <see cref="ViewModelBase(bool)"/> и <see cref="ViewModelBase(Dispatcher, bool)"/>,
        /// задано другое значение.</summary>
        public bool IsInDesignMode => settings.IsInDesignMode;

    }
}
