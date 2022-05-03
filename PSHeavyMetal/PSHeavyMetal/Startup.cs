using Microsoft.Extensions.DependencyInjection;
using System;

namespace PSHeavyMetal.Forms
{
    internal static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            ServiceProvider = new ServiceCollection().InitiliazeServices().
                InitializeRepositories().
                InitializeViewModels().
                BuildServiceProvider();
        }
    }
}