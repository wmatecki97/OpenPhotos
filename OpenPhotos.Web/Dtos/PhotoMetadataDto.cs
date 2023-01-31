using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Web.Dtos
{
    public class PhotoMetadataDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
