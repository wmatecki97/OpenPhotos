using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.Interfaces.Repositories;

public interface IPhotosRepository
{
    Task<List<PhotoMetadata>> GetAll();
    Task<PhotoMetadata> GetByName(string name);
    Task<List<PhotoMetadata>> GetByTag(string tag);
    Task<PhotoMetadata[]> GetTopLatestPhotos(int number);
    Task Add(PhotoMetadata photo);
    Task SaveChangesAsync();
    void RemoveByName(PhotoMetadata image);
}