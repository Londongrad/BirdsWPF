using BirdsWPF.Core;

namespace BirdsWPF.Services.Abstracts
{
    public interface INavigationService
    {
        ObservableObject CurrentView { get; }
        void NavigateTo(ObservableObject viewModel);
    }
}
