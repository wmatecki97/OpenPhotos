using Newtonsoft.Json;
using OpenPhotos.Contracts;
using OpenPhotos.Core.Interfaces;
using RabbitMQ.Client;
using System.Text;
using Constants = OpenPhotos.Contracts.Constants;

namespace OpenPhotos.Core.Messaging
{
    internal class RabbitPublisher : IMessagePublisher
    {
        private ConnectionFactory factory;

        public RabbitPublisher()
        {
            factory = new ConnectionFactory();
            factory.HostName = Configuration.GetRabbitHost();
            factory.VirtualHost = "/";
            factory.Port = Configuration.GetRabbitPort();
            factory.UserName = Configuration.GetRabbitUser();
            factory.Password = Configuration.GetRabbitPassword();
        }

        public void PublishSaveFileMessage(FileSaveRequest data)
        {
            byte[] content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();
            channel.ExchangeDeclare(Constants.SaveFileExchange, "fanout", true, false);
            channel.BasicPublish(Constants.SaveFileExchange, string.Empty, null, content);
        }
    }
}
