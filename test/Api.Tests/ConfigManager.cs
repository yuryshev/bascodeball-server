using Microsoft.Extensions.Configuration;

namespace Api.Tests;

public static class ConfigManager
{
        static ConfigManager()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("appsettings.ApiTests.json", false).AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public static IConfiguration Configuration { get; }
}