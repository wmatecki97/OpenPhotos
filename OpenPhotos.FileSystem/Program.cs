using Newtonsoft.Json;
using OpenPhotos.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Constants = OpenPhotos.Contracts.Constants;

namespace OpenPhotos.FileSystem
{
    internal class Program
    {

        private static IModel channel;

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = Configuration.GetRabbitHost();
            factory.VirtualHost = "/";
            factory.Port = Configuration.GetRabbitPort();
            factory.UserName = Configuration.GetRabbitUser();
            factory.Password = Configuration.GetRabbitPassword();

            var conn = factory.CreateConnection();
            channel = conn.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += SaveFileRequestRecieved;

            channel.QueueDeclare(Constants.SaveFileQueue, true, false, false);
            channel.ExchangeDeclare(Constants.SaveFileExchange, "fanout", true, false);
            channel.QueueBind(Constants.SaveFileQueue, Constants.SaveFileExchange, string.Empty, new Dictionary<string, object>());
            channel.BasicConsume(Constants.SaveFileQueue, false, consumer);

            Console.WriteLine("Waiting for saveFile requests");
            Console.ReadKey();
        }

        private static void SaveFileRequestRecieved(object? sender, BasicDeliverEventArgs e)
        {
            using var fileSystem = new FtpFileSystemWriter();
            string content = Encoding.UTF8.GetString(e.Body.ToArray());
            var request = JsonConvert.DeserializeObject<FileSaveRequest>(content);
            fileSystem.SaveFile(request.Name, request.Content);
            channel.BasicAck(e.DeliveryTag, false);
        }
    }
}