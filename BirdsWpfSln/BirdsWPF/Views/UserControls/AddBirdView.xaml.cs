using BirdsViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BirdsWPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ABView.xaml
    /// </summary>
    public partial class AddBirdView : UserControl
    {
        public AddBirdView()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddBirdViewModel? vm = (AddBirdViewModel)e.NewValue;
            vm?.Clear();
        }
    }
}
