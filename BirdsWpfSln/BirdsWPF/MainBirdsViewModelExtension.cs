using BirdsRepository;
using BirdsViewModels;
using System.Windows.Markup;

namespace BirdsWPF
{
    [MarkupExtensionReturnType(typeof(MainBirdsViewModel))]
    public class MainBirdsViewModelExtension : MarkupExtension
    {
        public string? DbFileName { get; set; }

        public MainBirdsViewModelExtension(string? dbFileName)
        {
            DbFileName = dbFileName;
        }

        public MainBirdsViewModelExtension()
        { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new MainBirdsViewModel(new DbBirdsRepository (DbFileName!));
        }
    }
}
