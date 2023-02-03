using Microsoft.Extensions.Configuration;
using OpenPhotos.Contracts;

namespace OpenPhotos.Core;

public static class Configuration
{
    private static readonly IConfigurationRoot config;

    static Configuration()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        config = builder.Build();
    }

    public static string GetImaggaApiKey()
    {
        return config["imaggaApiKey"];
    }

    public static string GetImaggaApiSecret()
    {
        return config["imaggaApiSecret"];
    }

    public static string GetFtpLogin()
    {
        return config["ftpLogin"];
    }

    public static string GetFtpPassword()
    {
        return config["ftpPassword"];
    }

    public static string GetOpenPhotosFolderPath()
    {
        return config["openPhotosFolderPath"] ?? "G/OpenPhotos";
    }

    public static string GetRabbitHost()
    {
        return config["RABBIT_HOST"] ?? "localhost";
    }

    public static int GetRabbitPort()
    {
        var portString = config["RABBIT_PORT"] ?? "5672";
        if (int.TryParse(portString, out var port)) return port;

        throw new InvalidConfigurationException("RabbitPort specified in the configuration is invalid");
    }

    public static string GetRabbitUser()
    {
        return config["RABBIT_USER"] ?? "guest";
    }

    public static string GetRabbitPassword()
    {
        return config["RABBIT_PASSWORD"] ?? "guest";
    }
}