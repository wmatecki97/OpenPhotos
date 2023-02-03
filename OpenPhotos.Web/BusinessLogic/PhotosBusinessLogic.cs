using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces;
using OpenPhotos.Core.Interfaces.Repositories;
using OpenPhotos.Web.Dtos;
using OpenPhotos.Web.Interfaces;

namespace OpenPhotos.Web.BusinessLogic;

public class PhotosBusinessLogic : IPhotosBusinessLogic
{
    private readonly IFileMetadataReader _fileMetadataReader;
    private readonly IFileSystem _fileSystem;
    private readonly IPhotosRepository _photosRepository;

    public PhotosBusinessLogic(
        IPhotosRepository photosRepository,
        IFileMetadataReader fileMetadataReader,
        IFileSystem fileSystem)
    {
        this._photosRepository = photosRepository;
        this._fileMetadataReader = fileMetadataReader;
        this._fileSystem = fileSystem;
    }

    public byte[] GetImageBytes(string imageName)
    {
        var image = _fileSystem.GetFile(imageName);
        return image;
    }

    public async Task<PhotoMetadata[]> GetMostCurrentPhotosAsync(int number)
    {
        var photos = await _photosRepository.GetTopLatestPhotos(number);
        return photos;
    }

    public async Task UploadPhoto(PhotoUploadDto upload)
    {
        var photoMetadata = _fileMetadataReader.GetFileMetadata(upload.Metadata);
        photoMetadata.Name =
            $"{upload.CreatedDate.Year}-{upload.CreatedDate.Month}-{upload.CreatedDate.Day}-{upload.Name}";

        //todo tags
        await _photosRepository.Add(photoMetadata);

        _fileSystem.SaveFile(photoMetadata.Name, upload.PhotoBytes);

        await _photosRepository.SaveChangesAsync();
    }
}