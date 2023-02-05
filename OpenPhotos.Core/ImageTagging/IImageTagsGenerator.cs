using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.ImageTagging.ImaggaModels;

namespace OpenPhotos.Core.ImageTagging;

public interface IImageTagsGenerator
{
    Task<TagResult[]> GetTagsForImage(byte[] imageBytes, string fileName);
}