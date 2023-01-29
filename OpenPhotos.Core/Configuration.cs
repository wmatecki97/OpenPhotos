using Microsoft.Extensions.Configuration;

namespace OpenPhotos.Core
{
    internal static class Configuration
    {
        private static IConfigurationRoot config;
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
    }
}
