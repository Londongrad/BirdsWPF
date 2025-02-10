using BirdsCommonStandard;
using BirdsRepository;
using BirdsViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
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
            string dbFileName = DbFileName!;
            IBirdsModel model = new BirdsDbModel(() => new BirdsAndSpeciesDbContext(dbFileName!));
            return new MainBirdsViewModel(model, ViewModelSettings);
        }

        private static readonly ConditionalWeakTable<ICommand, EventHandler> rs = new ConditionalWeakTable<ICommand, EventHandler>();
        public static ViewModelSettings ViewModelSettings { get; } = new ViewModelSettings
            (
                command =>
                {
                    if (Application.Current.Dispatcher.CheckAccess())
                    {
                        return true;
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(command.RaiseCanExecuteChanged);
                        return false;
                    }
                },
                command =>
                {
                    EventHandler requerySuggested = delegate { command.RaiseCanExecuteChanged(); };
                    CommandManager.RequerySuggested += requerySuggested;
                    rs.Add(command, requerySuggested);
                },
                _ => true,
                DesignerProperties.GetIsInDesignMode(new DependencyObject())
            );

    }
}
