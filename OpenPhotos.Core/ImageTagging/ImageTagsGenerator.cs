using System.Text;
using Newtonsoft.Json;
using OpenPhotos.Core.ImageTagging.ImaggaModels;
using RestSharp;

namespace OpenPhotos.Core.ImageTagging;

public class ImageTagsGenerator : IImageTagsGenerator
{
    public async Task<TagResult[]> GetTagsForImage(byte[] imageBytes, string fileName)
    {
        var apiKey = Configuration.GetImaggaApiKey();
        var apiSecret = Configuration.GetImaggaApiSecret();

        var basicAuthValue =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiKey, apiSecret)));

        var client = new RestClient("https://api.imagga.com/v2/tags");

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", string.Format("Basic {0}", basicAuthValue));
        request.AddFile("image", imageBytes, fileName);

        var response = await client.ExecuteAsync(request);
        var result = JsonConvert.DeserializeObject<ImaggaTagResponseModel>(response.Content);

        return result.Result.Tags.Select(t => new TagResult
        {
            Value = t.TagValue,
            Confidence = t.Confidence,
        }).ToArray();
    }
}