using BirdsRepository;
using BirdsViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseSqlite(DbFileName).Options;
            return new MainBirdsViewModel(new DbBirdsRepository(DbFileName!, options));
            //_ = optionsBuilder.UseSqlite(DbFullName);

            //optionsBuilder
            //    .EnableSensitiveDataLogging()
            //    .UseSqlite($"Data Source={DbFullName}");

            //optionsBuilder.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]);

            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-9OKU3FE\\SQLEXPRESS;Initial Catalog=Birds;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }
    }
}
