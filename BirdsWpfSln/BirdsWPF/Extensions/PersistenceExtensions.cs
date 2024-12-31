using BirdsCommon;
using BirdsWPF.Data;
using BirdsWPF.Repositories.Abstract;
using BirdsWPF.Repositories.Bases;
using BirdsWPF.Services.Bases;
using BirdsWPF.ViewModels;
using BirdsWPF.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace BirdsWPF.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            #region [ Windows ]
            services
                .AddScoped<MainWindow>();
            #endregion

            #region [ ViewModels ]
            services
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<AddBirdViewModel>()
                .AddTransient<BirdsViewModel>();
            #endregion

            #region [ Repositories ]
            services
                .AddSingleton<IBirdRepository, BirdRepository>();
            #endregion

            #region [ Services ]
            services
                .AddSingleton<INavigationService, NavigationService>();
            #endregion

            #region [ DBContext ]
            services.AddSingleton<ApplicationDbContext>();
            #endregion

            return services;
        }
    }
}
