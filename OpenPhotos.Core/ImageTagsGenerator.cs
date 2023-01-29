using Newtonsoft.Json;
using OpenPhotos.Core.ImaggaModels;
using RestSharp;

namespace OpenPhotos.Core
{
    public class ImageTagsGenerator
    {
        public string[] GetTagsForImage(string imagePath)
        {
            var fileContent = File.ReadAllText(imagePath);
            string apiKey = Configuration.GetImaggaApiKey();
            string apiSecret = Configuration.GetImaggaApiSecret();

            string basicAuthValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));

            var client = new RestClient("https://api.imagga.com/v2/tags");

            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));
            request.AddFile("image", imagePath);

            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            var result = JsonConvert.DeserializeObject<ImaggaTagResponseModel>(response.Content);

            return result.Result.Tags.Select(t => t.TagValue).ToArray();
        }
    }
}