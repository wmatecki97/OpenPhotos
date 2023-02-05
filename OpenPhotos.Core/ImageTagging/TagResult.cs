using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.ImageTagging
{
    public class TagResult
    {
        public string Value { get; set; }
        public double Confidence { get; set; }
        public PhotoMetadata Photo { get; set; }
    }
}
