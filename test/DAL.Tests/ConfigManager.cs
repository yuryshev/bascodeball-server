using Microsoft.Extensions.Configuration;

namespace DAL.Tests;

public static class ConfigManager
{
    static ConfigManager()
    {
        var configBuilder = new ConfigurationBuilder();

        configBuilder.AddJsonFile("appsettings.DAL.Tests.json", false).AddEnvironmentVariables();

        Configuration = configBuilder.Build();
    }

    public static IConfiguration Configuration { get; }
}