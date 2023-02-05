using Microsoft.Extensions.Configuration;
using OpenPhotos.Contracts;

namespace OpenPhotos.Core;

public static class Configuration
{
    private static readonly IConfigurationRoot Config;

    static Configuration()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        Config = builder.Build();
    }

    public static string GetConnectionString()
    {
        return Config["openPhotosConnectionString"] ?? "User ID=admin;Password=admin;Host=localhost;Port=5432;Database=OpenPhotos;";
    }

    public static string GetImaggaApiKey()
    {
        return Config["imaggaApiKey"];
    }

    public static string GetImaggaApiSecret()
    {
        return Config["imaggaApiSecret"];
    }

    public static string GetFtpLogin()
    {
        return Config["ftpLogin"];
    }

    public static string GetFtpPassword()
    {
        return Config["ftpPassword"];
    }

    public static string GetOpenPhotosFolderPath()
    {
        return Config["openPhotosFolderPath"] ?? "G/OpenPhotos";
    }

    public static string GetRabbitHost()
    {
        return Config["RABBIT_HOST"] ?? "localhost";
    }

    public static int GetRabbitPort()
    {
        var portString = Config["RABBIT_PORT"] ?? "5672";
        if (int.TryParse(portString, out var port)) return port;

        throw new InvalidConfigurationException("RabbitPort specified in the configuration is invalid");
    }

    public static string GetRabbitUser()
    {
        return Config["RABBIT_USER"] ?? "guest";
    }

    public static string GetRabbitPassword()
    {
        return Config["RABBIT_PASSWORD"] ?? "guest";
    }
}