using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Web.Dtos;

namespace OpenPhotos.Web.Interfaces
{
    public interface IPhotosBusinessLogic
    {
        byte[] GetImageBytes(string imageName);
        Task<PhotoMetadata[]> GetMostCurrentPhotosAsync(int number);
        Task UploadPhoto(PhotoUploadDto photoDto);
    }
}