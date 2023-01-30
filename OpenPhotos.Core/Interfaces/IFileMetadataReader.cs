
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.FileProcessing;

namespace OpenPhotos.Core.Interfaces
{
    public interface IFileMetadataReader
    {
        PhotoMetadata GetFileMetadata(Dictionary<string, string> metadata);
    }
}