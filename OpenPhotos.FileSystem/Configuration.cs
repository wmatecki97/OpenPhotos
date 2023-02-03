using Microsoft.Extensions.Configuration;
using OpenPhotos.Contracts;

namespace OpenPhotos.FileSystem;

public static class Configuration
{
    private static readonly IConfigurationRoot Config;

    static Configuration()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        Config = builder.Build();
    }

    public static string GetFtpLogin()
    {
        return Config["ftpLogin"] ?? throw new InvalidConfigurationException();
    }

    public static string GetFtpPassword()
    {
        return Config["ftpPassword"] ?? throw new InvalidConfigurationException();
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

        throw new InvalidConfigurationException(
            $"RabbitPort specified in the configuration is invalid. Received value is {portString}");
    }

    public static string GetRabbitUser()
    {
        return Config["RABBIT_USER"] ?? "guest";
    }

    public static string GetRabbitPassword()
    {
        return Config["RABBIT_PASSWORD"] ?? "guest";
    }

    public static int GetMaxThumbnailSizeInBytes()
    {
        const int kbToBRatio = 1000;
        var sizeString = Config["MAX_THUMBNAIL_SIZE_KB"] ?? "400";
        if (int.TryParse(sizeString, out var size)) return size * kbToBRatio;

        throw new InvalidConfigurationException(
            $"Maximum thumbnail size in the configuration is not valid. Received value is {sizeString}");
    }
}