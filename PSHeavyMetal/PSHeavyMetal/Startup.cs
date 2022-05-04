using Microsoft.Extensions.DependencyInjection;
using System;

namespace PSHeavyMetal.Forms
{
    internal static class Startup
    {
        public static ServiceProvider Init()
        {
            return new ServiceCollection().InitiliazeServices().
                InitializeRepositories().
                InitializeViewModels().
                BuildServiceProvider();
        }
    }
}