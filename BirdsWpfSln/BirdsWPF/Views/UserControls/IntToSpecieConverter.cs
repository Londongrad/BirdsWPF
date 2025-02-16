using BirdsViewModels;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace BirdsWPF.Views.UserControls
{
    public class IntToSpecieConverter : IValueConverter
    {
        private static readonly Int32Converter converter = new Int32Converter();

        public MainBirdsViewModel? ViewModel { get; set; }
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int id)
            {
                id = (int)converter.ConvertFrom(null, culture, value)!;
            }

            return ViewModel?.GetSpecie(id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
