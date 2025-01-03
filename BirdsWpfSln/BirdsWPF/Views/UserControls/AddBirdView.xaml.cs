using BirdsCommon.Repository;
using BirdsViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
