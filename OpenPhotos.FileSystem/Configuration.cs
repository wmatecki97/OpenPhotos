using Microsoft.Extensions.Configuration;

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
}
