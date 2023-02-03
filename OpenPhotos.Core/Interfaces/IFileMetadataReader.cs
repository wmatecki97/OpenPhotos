using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.Interfaces;

public interface IFileMetadataReader
{
    PhotoMetadata GetFileMetadata(Dictionary<string, string> metadata);
}