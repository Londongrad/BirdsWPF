using System.ComponentModel;
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
            ICollectionView collectionView = (ICollectionView)e.Parameter;
            e.CanExecute = collectionView is not null &&  collectionView.CurrentPosition > 0;
        }

        private void OnForwardCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ICollectionView collectionView = (ICollectionView)e.Parameter;
            e.CanExecute = collectionView is not null &&  collectionView.CurrentPosition < collectionView.Cast<object>().Count() - 1;
        }

        private void OnBackExecute(object sender, ExecutedRoutedEventArgs e)
        {
            ICollectionView collectionView = (ICollectionView)e.Parameter;
            collectionView.MoveCurrentToPrevious();
        }

        private void OnForwardExecute(object sender, ExecutedRoutedEventArgs e)
        {
            ICollectionView collectionView = (ICollectionView)e.Parameter;
            collectionView.MoveCurrentToNext();
        }
    }
}
