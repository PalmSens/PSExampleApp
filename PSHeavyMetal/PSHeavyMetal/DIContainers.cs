using Microsoft.Extensions.DependencyInjection;
using PSHeavyMetal.Core.DataAccess;
using PSHeavyMetal.Core.Repositories;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.ViewModels;

namespace PSHeavyMetal.Forms
{
    public static class DIContainers
    {
        public static IServiceCollection InitiliazeServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection InitializeViewModels(this IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<AddUserViewModel>();
            return services;
        }

        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDataOperations, LiteDbDataOperations>();
            services.AddSingleton<IDeviceRepository, DeviceRepository>();
            services.AddSingleton<IMeasurementRepository, MeasurementRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            return services;
        }
    }
}