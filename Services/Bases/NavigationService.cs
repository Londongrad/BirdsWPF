using BirdsWPF.Core;
using BirdsWPF.Services.Abstracts;
using BirdsWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BirdsWPF.Services.Bases
{
    public class NavigationService() : ObservableObject, INavigationService
    {
        #region [ Fields ]
        private ObservableObject _currentView = App.ServiceProvider!.GetRequiredService<AddBirdViewModel>();
        #endregion

        #region [ Properties ]
        public ObservableObject CurrentView { get => _currentView; private set => Set(ref _currentView, value); }
        #endregion

        #region [ Methods ]
        public void NavigateTo(ObservableObject viewModel)
        {
            CurrentView = viewModel;
        }
        #endregion
    }
}
