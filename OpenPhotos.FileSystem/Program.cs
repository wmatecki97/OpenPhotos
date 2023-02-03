using RabbitMQ.Client;

namespace OpenPhotos.FileSystem;

internal class Program
{
    private static void Main()
    {
        var factory = new ConnectionFactory();
        factory.HostName = Configuration.GetRabbitHost();
        factory.VirtualHost = "/";
        factory.Port = Configuration.GetRabbitPort();
        factory.UserName = Configuration.GetRabbitUser();
        factory.Password = Configuration.GetRabbitPassword();

        var fullQualityFileSaver = new FullQualityFileSaver();
        fullQualityFileSaver.Register(factory);

        var thumbnailFileSaver = new ThumbnailFileSaver();
        thumbnailFileSaver.Register(factory);

        Console.WriteLine("Waiting for saveFile requests");
        Console.ReadKey();
    }
}