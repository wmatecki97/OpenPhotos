using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces;
using OpenPhotos.Core.Interfaces.Repositories;
using OpenPhotos.Web.Dtos;
using OpenPhotos.Web.Interfaces;
using System.Text;

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

        public async Task UploadPhoto(PhotoUploadDto photoDto)
        {
            var photoMetadata = fileMetadataReader.GetFileMetadata(photoDto.Metadata);
            photoMetadata.Name = photoDto.Name;
            //todo tags
            await photosRepository.Add(photoMetadata);

            var fileBytes = Encoding.UTF8.GetBytes(photoDto.PhotoBase64);
            var allfiles = fileSystem.GetAllFiles();
            fileSystem.SaveFile(photoDto.Name, fileBytes);

            await photosRepository.SaveChangesAsync();
        }
    }
}
