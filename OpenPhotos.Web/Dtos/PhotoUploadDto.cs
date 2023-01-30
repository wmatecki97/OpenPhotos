using System.Buffers.Text;

namespace OpenPhotos.Web.Dtos
{
    public class PhotoUploadDto
    {
        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string PhotoBase64 { get; set; }
    }
}
