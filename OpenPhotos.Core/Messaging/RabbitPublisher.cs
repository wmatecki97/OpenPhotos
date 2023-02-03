using System.Text;
using Newtonsoft.Json;
using OpenPhotos.Contracts;
using OpenPhotos.Core.Interfaces;
using RabbitMQ.Client;
using Constants = OpenPhotos.Contracts.Constants;

namespace OpenPhotos.Core.Messaging;

internal class RabbitPublisher : IMessagePublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitPublisher()
    {
        _factory = new ConnectionFactory();
        _factory.HostName = Configuration.GetRabbitHost();
        _factory.VirtualHost = "/";
        _factory.Port = Configuration.GetRabbitPort();
        _factory.UserName = Configuration.GetRabbitUser();
        _factory.Password = Configuration.GetRabbitPassword();
    }

    public void PublishSaveFileMessage(FileSaveRequest data)
    {
        var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        using var conn = _factory.CreateConnection();
        using var channel = conn.CreateModel();
        channel.ExchangeDeclare(Constants.SaveFileExchange, "fanout", true);
        channel.BasicPublish(Constants.SaveFileExchange, string.Empty, null, content);
    }
}