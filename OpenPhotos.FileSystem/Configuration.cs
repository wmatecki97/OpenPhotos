using Microsoft.Extensions.Configuration;
using OpenPhotos.Contracts;

namespace OpenPhotos.FileSystem
{
    public static class Configuration
    {
        private static IConfigurationRoot config;
        static Configuration()
        {
            var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

            config = builder.Build();
        }

        public static string GetFtpLogin()
        {
            return config["ftpLogin"] ?? throw new InvalidConfigurationException();
        }

        public static string GetFtpPassword()
        {
            return config["ftpPassword"] ?? throw new InvalidConfigurationException();
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
            if (int.TryParse(portString, out var port))
            {
                return port;
            }

            throw new InvalidConfigurationException($"RabbitPort specified in the configuration is invalid. Received value is {portString}");
        }

        public static string GetRabbitUser()
        {
            return config["RABBIT_USER"] ?? "guest";
        }
        public static string GetRabbitPassword()
        {
            return config["RABBIT_PASSWORD"] ?? "guest";
        }

        public static int GetMaxThumbnailSizeInBytes()
        {
            const int kbToBRatio = 1000;
            var sizeString = config["MAX_THUMBNAIL_SIZE_KB"] ?? "400";
            if (int.TryParse(sizeString, out var size))
            {
                return size*kbToBRatio;
            }

            throw new InvalidConfigurationException($"Maximum thumbnail size in the configuration is not valid. Received value is {sizeString}");
        }
    }
}
