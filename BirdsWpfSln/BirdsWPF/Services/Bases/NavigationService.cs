using BirdsCommon;

namespace BirdsWPF.Services.Bases
{
    public class NavigationService() : ViewModelBase, INavigationService
    {

        #region [ Properties ]
        public object? Current { get => Get<object>(); private set => Set(value); }
        #endregion

        #region [ Methods ]
        public void NavigateTo(object? viewModel)
        {
            Current = viewModel;
        }
        #endregion
    }
}
