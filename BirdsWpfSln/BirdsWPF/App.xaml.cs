using BirdsCommon;
using BirdsCommonStandard;
using BirdsViewModels;
using BirdsWPF.Views.Windows;
using System.Windows;

namespace BirdsWPF
{
    public partial class App : Application
    {
        public App()
        {
            ViewModelSettings.SetDefault(MainBirdsViewModelExtension.ViewModelSettings);
            Startup += async (s, e) =>
            {

                MainBirdsViewModel vm = (MainBirdsViewModel)FindResource("mainVM");
                vm.Birds.EnableCollectionSynchronization();
                vm.Species.EnableCollectionSynchronization();
                await vm.LoadAsync();
            };
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
