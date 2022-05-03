using Microsoft.Extensions.DependencyInjection;
using PSHeavyMetal.Core.DataAccess;
using PSHeavyMetal.Core.Repositories;
using PSHeavyMetal.Core.Services;

namespace PSHeavyMetal.Forms
{
    public static class DIContainers
    {
        public static IServiceCollection InitiliazeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection InitializeViewModels(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDataOperations, RavenDbDataOperations>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}