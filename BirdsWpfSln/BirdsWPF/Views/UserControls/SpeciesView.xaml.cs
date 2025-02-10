using BirdsCommonStandard;
using BirdsViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BirdsWPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SpeciesView.xaml
    /// </summary>
    public partial class SpeciesView : UserControl
    {
        public SpeciesView()
        {
            InitializeComponent();
        }

        private void OnSelectedSpecieChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource birds = (CollectionViewSource)FindResource("birds");
            HashSet<int> ids = speciesList.SelectedItems.OfType<IdSpecieVM>().Select(s => s.Id).ToHashSet();
            FilterEventHandler? filter = Application.Current.Resources["birdsFilter"] as FilterEventHandler;
            if (filter is not null)
            {
                birds.Filter -= filter;
            }
            if (ids.Count > 0)
            {
                filter = (s, e) =>
                {
                    IdBirdVM bird = (IdBirdVM)e.Item;
                    e.Accepted &= ids.Contains(bird.ModelEntity.SpecieId);
                };
                Application.Current.Resources["birdsFilter"] = filter;
                birds.Filter += filter;
            }
        }
    }
}
