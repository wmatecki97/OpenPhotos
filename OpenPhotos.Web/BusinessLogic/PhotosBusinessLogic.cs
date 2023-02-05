using AutoMapper;
using OpenPhotos.Contracts;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.ImageTagging;
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
    private readonly IImageTagsGenerator _tagsGenerator;
    private readonly IMapper _mapper;

    public PhotosBusinessLogic(
        IPhotosRepository photosRepository,
        IFileMetadataReader fileMetadataReader,
        IFileSystem fileSystem, IImageTagsGenerator tagsGenerator, IMapper mapper)
    {
        _photosRepository = photosRepository;
        _fileMetadataReader = fileMetadataReader;
        _fileSystem = fileSystem;
        _tagsGenerator = tagsGenerator;
        _mapper = mapper;
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
        var photoMetadata = _fileMetadataReader.GetFileMetadata(upload.PhotoBytes);
        photoMetadata.Name =
            $"{upload.CreatedDate.Year}-{upload.CreatedDate.Month}-{upload.CreatedDate.Day}-{upload.Name}";

        _fileSystem.SaveFile(photoMetadata.Name, upload.PhotoBytes);

        await AddTagsAndSaveEntity(photoMetadata, upload.PhotoBytes);
    }

    public async Task RemoveInconsistencies(bool acceptPotentialDataLoss = false)
    {
        var dbEntriesTask = _photosRepository.GetAll();
        var files = _fileSystem.GetAllFiles(Constants.FullQualityFolderName);
        var thumbnails = _fileSystem.GetAllFiles(Constants.ThumbnailsFolderName);
        var dbEntries = await dbEntriesTask;

        var allImages = files.Union(thumbnails).ToList();
        await RecoverMissingDbEntries(allImages, dbEntries);

        RecoverThumbnailsFromImages(files, thumbnails);

        if (acceptPotentialDataLoss)
        {
            SaveThumbnailsAsMissingFullQualityImages(thumbnails, files);
            await RemoveDbEntitiesWithMissingImages(dbEntries, allImages);
        }
    }

    public Task RegenerateAllThumbnails()
    {
        _fileSystem.DeleteAllThumbnails();
        return RemoveInconsistencies();
    }

    public Task<PhotoMetadata[]> GetTopPhotosByText(string text, int i)
    {
        return _photosRepository.GetByTag(text);
    }

    private async Task AddTagsAndSaveEntity(PhotoMetadata photoMetadata, byte[] uploadPhotoBytes)
    {
        var tags = await _tagsGenerator.GetTagsForImage(uploadPhotoBytes, photoMetadata.Name);
        photoMetadata.Tags = tags.Select(_mapper.Map<Tag>).ToList();

        //await _photosRepository.Add(photoMetadata);
        await _photosRepository.SaveChangesAsync();
    }

    private async Task RemoveDbEntitiesWithMissingImages(List<PhotoMetadata> dbEntries, List<string> allImages)
    {
        var missingImages = dbEntries.Where(d => !allImages.Any(i => i.StartsWith(d.Name))).ToArray();
        foreach (var image in missingImages) _photosRepository.RemoveByName(image);

        await _photosRepository.SaveChangesAsync();
    }

    private void SaveThumbnailsAsMissingFullQualityImages(List<string> thumbnails, List<string> files)
    {
        var missingFullQualityImage = thumbnails.Where(f => !files.Contains(f)).ToArray();
        foreach (var missingImage in missingFullQualityImage)
        {
            var image = _fileSystem.GetFile(missingImage);
            _fileSystem.SaveFile(missingImage, image);
            GC.Collect();
        }
    }

    private void RecoverThumbnailsFromImages(List<string> files, List<string> thumbnails)
    {
        var missingThumbnails = files.Where(f => !thumbnails.Contains(f)).ToArray();
        foreach (var missingThumbnail in missingThumbnails)
        {
            var image = _fileSystem.GetFile(missingThumbnail, true);
            _fileSystem.SaveFile(missingThumbnail, image);
            GC.Collect();
        }
    }

    private async Task RecoverMissingDbEntries(List<string> allImages, List<PhotoMetadata> dbEntries)
    {
        var missingDbEntries = allImages.Where(f => !dbEntries.Any(d => f.StartsWith(d.Name))).ToArray();
        var dbEntriesToAdd = missingDbEntries.Select(i => new PhotoMetadata { Name = i }).ToList();
        foreach (var entity in dbEntriesToAdd)
        {
            var fileContent = _fileSystem.GetFile(entity.Name, true);
            await AddTagsAndSaveEntity(entity, fileContent);
        }
    }

}