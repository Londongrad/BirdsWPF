using BirdsCommon;
using Microsoft.Extensions.DependencyInjection;

namespace BirdsWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public MainWindowViewModel()
        {
            // Здесь нужно прописат значение по умолчанию для Navigation.
            // Можно какую-то демо реализацию INavigationService.
        }

        #region [ Properties ]
        public INavigationService Navigation { get; }
        #endregion

        #region [ Commands ]
        public RelayCommand ToABViewCommand => GetCommand
            (
                () => Navigation.NavigateTo(App.ServiceProvider!.GetRequiredService<AddBirdViewModel>()
            ));


        public RelayCommand ToBirdsViewCommand => GetCommand
            (
                () => Navigation.NavigateTo(App.ServiceProvider!.GetRequiredService<BirdsViewModel>()
            ));
        #endregion
    }
}
