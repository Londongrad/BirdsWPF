using System.Windows.Controls;
using System.Windows.Input;

namespace BirdsWPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ListBirdView.xaml
    /// </summary>
    public partial class ListBirdView : UserControl
    {
        public ListBirdView()
        {
            InitializeComponent();
        }

        private void OnBackCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listBirds.SelectedIndex > 0;
        }

        private void OnForwardCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listBirds.SelectedIndex < listBirds.Items.Count - 1;
        }

        private void OnBackExecute(object sender, ExecutedRoutedEventArgs e)
        {
            listBirds.SelectedIndex--;
        }

        private void OnForwardExecute(object sender, ExecutedRoutedEventArgs e)
        {
            listBirds.SelectedIndex++;
        }
    }
}
