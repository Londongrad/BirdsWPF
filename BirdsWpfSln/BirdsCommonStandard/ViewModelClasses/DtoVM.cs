namespace BirdsCommonStandard
{
    /// <summary>Оболочка с уведомлением для сущности Модели.</summary>
    /// <typeparam name="TDto">Тип сущности Модели.</typeparam>
    public class DtoVM<TDto> : ViewModelBase
    {
        private TDto _entity;

        public DtoVM(ViewModelSettings settings)
            : base(settings)
        { }
        public DtoVM()
            : this(ViewModelSettings.Default)
        { }

        /// <summary>Сущность Модели.</summary>
        public TDto ModelEntity => _entity;

        /// <summary>Заменяет сущность. После замены подымает событие <see cref="BaseInpc.PropertyChanged"/> для всех свойств.</summary>
        /// <param name="entity"></param>
        public virtual void SetEntity(TDto entity)
        {
            _entity = entity;
            RaisePropertyChanged(AllPropertyChangedEventArgs);
        }
    }
}
