using System.Text;
using Newtonsoft.Json;
using OpenPhotos.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Constants = OpenPhotos.Contracts.Constants;

namespace OpenPhotos.FileSystem;

internal class ThumbnailFileSaver
{
    private static IModel _channel;

    public void Register(ConnectionFactory factory)
    {
        var conn = factory.CreateConnection();
        _channel = conn.CreateModel();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += SaveFileRequestRecieved;

        _channel.BasicQos(0, 1, false);
        _channel.QueueDeclare(Constants.SaveThumbnailQueue, true, false, false);
        _channel.ExchangeDeclare(Constants.SaveFileExchange, "fanout", true);
        _channel.QueueBind(Constants.SaveThumbnailQueue, Constants.SaveFileExchange, string.Empty,
            new Dictionary<string, object>());
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
        const int resizedBiggerDimensionPixelCount = 380;
        const int jpegQuality = 70;

        using var inputStream = new MemoryStream(fullSizeImage);//todo configuration
        var encoder = new JpegEncoder { Quality = jpegQuality };//todo configuration

        var outputBytes = Array.Empty<byte>();
        try
        {
            int height = 0, width = 0;
            var image = Image.Load(inputStream);
            if (image.Height > image.Width)
            {
                height = resizedBiggerDimensionPixelCount;
            }
            else
            {
                width = resizedBiggerDimensionPixelCount;
            }

            using var outputStream = new MemoryStream();
            image.Mutate(x => x.Resize(width, height));
            image.Save(outputStream, encoder);
            outputBytes = outputStream.ToArray();
        }
        catch (UnknownImageFormatException)
        {
            //it isn't an image, skip processing, save as is
        }

        GC.Collect();
        return outputBytes;
    }
}