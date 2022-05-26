using Microsoft.Extensions.Configuration;

namespace API.Tests;

public static class ConfigManager
{
    static ConfigManager()
    {
        var configBuilder = new ConfigurationBuilder();

        configBuilder.AddJsonFile("appsettings.APITests.json", false).AddEnvironmentVariables();

        Configuration = configBuilder.Build();
    }

    public static IConfiguration Configuration { get; }
}