using Newtonsoft.Json;
using OpenPhotos.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Constants = OpenPhotos.Contracts.Constants;

namespace OpenPhotos.FileSystem
{
    internal class FullQualityFileSaver
    {
        private static IModel _channel;

        public void Register(ConnectionFactory factory)
        {
            var conn = factory.CreateConnection();
            _channel = conn.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += SaveFileRequestRecieved;

            _channel.QueueDeclare(Constants.SaveFileQueue, true, false, false);
            _channel.ExchangeDeclare(Constants.SaveFileExchange, "fanout", true, false);
            _channel.QueueBind(Constants.SaveFileQueue, Constants.SaveFileExchange, string.Empty, new Dictionary<string, object>());
            _channel.BasicConsume(Constants.SaveFileQueue, false, consumer);
        }

        private static void SaveFileRequestRecieved(object? sender, BasicDeliverEventArgs e)
        {
            using var fileSystem = new FtpFileSystemWriter();
            var content = Encoding.UTF8.GetString(e.Body.ToArray());
            var request = JsonConvert.DeserializeObject<FileSaveRequest>(content);
            var name = $"{Constants.FullQualityFolderName}/{request.Name}";
            fileSystem.SaveFile(name, request.Content);
            _channel.BasicAck(e.DeliveryTag, false);
        }
    }
}
