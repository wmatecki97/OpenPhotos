using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces;

namespace OpenPhotos.Core.FileProcessing;

internal class FileMetadataReader : IFileMetadataReader
{
    public PhotoMetadata GetFileMetadata(Dictionary<string, string> metadata)
    {
        //todo
        var result = new PhotoMetadata
        {
            DateTaken = DateTime.Now
        };

        return result;
    }
}