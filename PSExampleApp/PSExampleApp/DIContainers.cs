using Microsoft.Extensions.DependencyInjection;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Core.DataAccess;
using PSExampleApp.Core.Repositories;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.ViewModels;
using Xamarin.Forms;

namespace PSExampleApp.Forms
{
    public static class DIContainers
    {
        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDataOperations, LiteDbDataOperations>();
            services.AddSingleton<IAppConfigurationRepository, AppConfigurationRepository>();
            services.AddSingleton<IMeasurementRepository, MeasurementRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection InitializeViewModels(this IServiceCollection services)
        {
            services.AddTransient<MeasurementPlotViewModel>();
            services.AddTransient<ConfigureTitleViewModel>();
            services.AddTransient<SelectMeasurementViewModel>();
            services.AddTransient<MeasurementFinishedViewModel>();
            services.AddTransient<RunMeasurementViewModel>();
            services.AddTransient<ConfigureMeasurementViewModel>();
            services.AddTransient<ConfigureApplicationViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<SelectDeviceViewModel>();
            services.AddTransient<PrepareMeasurementViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SensorDetectionViewModel>();
            services.AddTransient<MeasurementDataViewModel>();

            // The next view models are added as scoped since they are used more persistant in the application.
            services.AddScoped<TitleViewModel>();
            services.AddScoped<StatusBarViewModel>();
            return services;
        }

        public static IServiceCollection InitiliazeServices(this IServiceCollection services)
        {
            services.AddSingleton<IAppConfigurationService, AppConfigurationService>();
            services.AddSingleton<IShareService, ShareService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton(DependencyService.Resolve<IPermissionService>());
            services.AddSingleton(DependencyService.Resolve<IMessageService>());
            services.AddSingleton(DependencyService.Resolve<ILoadAssetsService>());
            services.AddSingleton(DependencyService.Resolve<InstrumentService>());
            services.AddSingleton(DependencyService.Resolve<ILoadSavePlatformService>());
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<IMeasurementService, MeasurementService>();
            return services;
        }
    }
}