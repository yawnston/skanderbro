using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skanderbro.Configuration;

namespace Skanderbro
{
    internal static class Program
    {
        private static IConfigurationRoot Configuration;

        private static void Main(string[] _)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<BotSecrets>()
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var bot = serviceProvider.GetService<SkanderbroBot>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BotSecrets>(Configuration.GetSection(nameof(BotSecrets)))
                .AddOptions()
                .AddSingleton<SkanderbroBot>()
                .BuildServiceProvider();
        }
    }
}
