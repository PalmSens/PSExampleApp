using Microsoft.Extensions.DependencyInjection;

namespace PSExampleApp.Forms
{
    internal static class Startup
    {
        public static ServiceProvider Init()
        {
            return new ServiceCollection().
                InitiliazeServices().
                InitializeRepositories().
                InitializeViewModels().
                BuildServiceProvider();
        }
    }
}