using Microsoft.Extensions.DependencyInjection;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.DataAccess;
using PSHeavyMetal.Core.Repositories;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.ViewModels;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms
{
    public static class DIContainers
    {
        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDataOperations, LiteDbDataOperations>();
            services.AddSingleton<IDeviceRepository, DeviceRepository>();
            services.AddSingleton<IMeasurementRepository, MeasurementRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection InitializeViewModels(this IServiceCollection services)
        {
            services.AddTransient<RunMeasurementViewModel>();
            services.AddTransient<ConfigureMeasurementViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<SelectDeviceViewModel>();
            services.AddTransient<PrepareMeasurementViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SensorDetectionViewModel>();
            services.AddTransient<DropDetectionViewModel>();
            services.AddTransient<TitleViewModel>();
            return services;
        }

        public static IServiceCollection InitiliazeServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton(DependencyService.Resolve<IPermissionService>());
            services.AddSingleton(DependencyService.Resolve<InstrumentService>());
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<IMeasurementService, MeasurementService>();
            return services;
        }
    }
}