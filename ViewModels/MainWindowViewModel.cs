using BirdsWPF.Core;
using BirdsWPF.Services.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace BirdsWPF.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region [ Fields ]
        private INavigationService? _navigation;
        #endregion

        public MainWindowViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public MainWindowViewModel(){ }

        #region [ Properties ]
        public INavigationService Navigation { get => _navigation!; private set => Set(ref _navigation, value); }
        #endregion

        #region [ Commands ]
        public RelayCommand ToABViewCommand => new RelayCommand
            (
                execute => Navigation.NavigateTo(App.ServiceProvider!.GetRequiredService<AddBirdViewModel>()
            ));
        public RelayCommand ToBirdsViewCommand => new RelayCommand
            (
                execute => Navigation.NavigateTo(App.ServiceProvider!.GetRequiredService<BirdsViewModel>()
            ));
        #endregion
    }
}
