using BirdsRepository;
using BirdsViewModels;
using System.Windows.Markup;

namespace BirdsWPF
{
    [MarkupExtensionReturnType(typeof(MainBirdsViewModel))]
    public class MainBirdsViewModelExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new MainBirdsViewModel(new  DbBirdsRepository ("birds.db"));
        }
    }
}
