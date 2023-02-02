using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces;
using OpenPhotos.Core.Interfaces.Repositories;
using OpenPhotos.Web.Dtos;
using OpenPhotos.Web.Interfaces;

namespace OpenPhotos.Web.BusinessLogic
{
    public class PhotosBusinessLogic : IPhotosBusinessLogic
    {
        private readonly IPhotosRepository photosRepository;
        private readonly IFileMetadataReader fileMetadataReader;
        private readonly IFileSystem fileSystem;

        public PhotosBusinessLogic(
            IPhotosRepository photosRepository,
            IFileMetadataReader fileMetadataReader,
            IFileSystem fileSystem)
        {
            this.photosRepository = photosRepository;
            this.fileMetadataReader = fileMetadataReader;
            this.fileSystem = fileSystem;
        }

        public byte[] GetImageBytes(string imageName)
        {
            var image = fileSystem.GetFile(imageName);
            return image;
        }

        public async Task<PhotoMetadata[]> GetMostCurrentPhotosAsync(int number)
        {
            var photos = await photosRepository.GetTopLatestPhotos(number);
            return photos;
        }

        public async Task UploadPhoto(PhotoUploadDto upload)
        {
            var photoMetadata = fileMetadataReader.GetFileMetadata(upload.Metadata);
            photoMetadata.Name = $"{upload.CreatedDate.Year}-{upload.CreatedDate.Month}-{upload.CreatedDate.Day}-{upload.Name}";
            
            //todo tags
            await photosRepository.Add(photoMetadata);

            fileSystem.SaveFile(photoMetadata.Name, upload.PhotoBytes);

            await photosRepository.SaveChangesAsync();
        }
    }
}
