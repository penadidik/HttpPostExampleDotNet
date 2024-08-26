using System;
using Microsoft.Extensions.Configuration;

public static class AppConfig
{
    // Singleton instance of the configuration
    private static IConfiguration? _configuration;

    // Initialize the configuration
    public static void Initialize(string jsonFilePath)
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Base path to look for the JSON file
            .AddJsonFile(jsonFilePath)
            .Build();
    }

    // Get the connection string from configuration
    public static string GetConnectionString(string key)
    {
        return _configuration.GetConnectionString(key);
    }

    // Example method to get a configuration value
    public static string GetAppSetting(string key)
    {
        return _configuration[key];
    }
}
