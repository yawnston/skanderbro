using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Skanderbro.Configuration;

namespace Skanderbro
{
    internal static class Program
    {
        private static IConfigurationRoot Configuration;

        private static async Task Main(string[] _)
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
            await bot.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>(LoggerFactory);

            services.AddOptions();
            services.Configure<BotSecrets>(Configuration.GetSection(nameof(BotSecrets)));

            services.AddSingleton<SkanderbroBot>();
        }

        private static Logger LoggerFactory(IServiceProvider _)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = logger;
            return logger;
        }
    }
}
