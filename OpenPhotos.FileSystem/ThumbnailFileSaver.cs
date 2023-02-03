using Newtonsoft.Json;
using OpenPhotos.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Constants = OpenPhotos.Contracts.Constants;
using SixLabors.ImageSharp.Processing;

namespace OpenPhotos.FileSystem
{
    internal class ThumbnailFileSaver
    {
        private static IModel _channel;

        public void Register(ConnectionFactory factory)
        {
            var conn = factory.CreateConnection();
            _channel = conn.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += SaveFileRequestRecieved;

            _channel.QueueDeclare(Constants.SaveThumbnailQueue, true, false, false);
            _channel.ExchangeDeclare(Constants.SaveFileExchange, "fanout", true, false);
            _channel.QueueBind(Constants.SaveThumbnailQueue, Constants.SaveFileExchange, string.Empty, new Dictionary<string, object>());
            _channel.BasicConsume(Constants.SaveThumbnailQueue, false, consumer);

            Console.WriteLine("Waiting for saveFile requests");
            Console.ReadKey();
        }

        private void SaveFileRequestRecieved(object? sender, BasicDeliverEventArgs e)
        {
            using var fileSystem = new FtpFileSystemWriter();
            var content = Encoding.UTF8.GetString(e.Body.ToArray());
            var request = JsonConvert.DeserializeObject<FileSaveRequest>(content);
            request.Content = GetImageThumbnail(request.Content);
            var name = $"{Constants.ThumbnailsFolderName}/{request.Name}";
            fileSystem.SaveFile(name, request.Content);
            _channel.BasicAck(e.DeliveryTag, false);
        }

        private byte[] GetImageThumbnail(byte[] fullSizeImage)
        {
            var jpegQuality = 100;
            using var inputStream = new MemoryStream(fullSizeImage);
            var image = Image.Load(inputStream, out var format);
            var encoder = new JpegEncoder { Quality = jpegQuality };
            byte[] outputBytes;
            var maxThumbnailSize = Configuration.GetMaxThumbnailSizeInBytes();

            do
            {
                using var outputStream = new MemoryStream();
                var width = (int)(image.Width * 0.9);
                image.Mutate(x => x.Resize(width, 0));
                image.Save(outputStream, encoder);
                outputBytes = outputStream.ToArray();
            } 
            while (outputBytes.Length > maxThumbnailSize);

            GC.Collect();
            return outputBytes;
        }
    }
}
