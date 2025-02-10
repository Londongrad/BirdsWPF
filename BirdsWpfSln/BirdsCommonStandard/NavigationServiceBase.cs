using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BirdsCommonStandard
{
    public abstract class NavigationServiceBase : ViewModelBase, INavigationService
    {



        /// <summary>Текущий выбранный объект. Не важно какой именно.
        /// Реализует INPC или нет, выполянет ли какие-то другие требования - ничто не важно.</summary>
        /// <remarks>При смене значения должно происходить уведомление через интерфейс <see cref="INotifyPropertyChanged"/>.</remarks>

        public object Current
        {
            get => Get<object>();
            protected set => Set(value);
        }

        /// <summary>Метод переключения текущего объекта.</summary>
        /// <param name="viewModel"></param>
        public void NavigateTo(object viewModel) => Current = viewModel;

        /// <summary>Команда вызывающая метод <see cref="NavigateTo"/> с реализацией по умолчанию.</summary>
        public RelayCommand NavigateToCommand => GetCommand<object>(NavigateTo);


        public RelayCommand<Type> NavigateToTypeCommand => (RelayCommand<Type>)GetCommand<Type>(NavigateToType);

        public void NavigateToType(Type type)
        {
            if (typeCreators.TryGetValue(type, out Func<object> creator))
            {
                NavigateTo(creator());
            }
            else
            {
                NavigateTo(null);
            }
        }

        public void AddCreator(Type type, Func<object> creator)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (creator is null) throw new ArgumentNullException(nameof(creator));

            typeCreators[type] = creator;
        }

        private static readonly Dictionary<Type, Func<object>> typeCreators = new Dictionary<Type, Func<object>>();

        protected NavigationServiceBase(ViewModelSettings settings)
            : base(settings)
        { }

        protected NavigationServiceBase()
        { }
    }
}
