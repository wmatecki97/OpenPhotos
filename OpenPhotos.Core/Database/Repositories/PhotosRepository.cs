using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces.Repositories;

namespace OpenPhotos.Core.Database.Repositories;

public class PhotosRepository : IPhotosRepository
{
    private readonly IOpenPhotosDbContext context;

    public PhotosRepository(IOpenPhotosDbContext dbContext)
    {
        context = dbContext;
    }

    public async Task Add(PhotoMetadata photo)
    {
        await context.Photos.AddAsync(photo);
    }

    public async Task<List<PhotoMetadata>> GetAll()
    {
        return await context.Photos.ToListAsync();
    }

    public async Task<PhotoMetadata> GetByName(string name)
    {
        return await context.Photos.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<List<PhotoMetadata>> GetByTag(string tag)
    {
        return await context.Photos.Include(p => p.Tags)
            .Where(p => p.Tags.Any(t => t.Value.Contains(tag, StringComparison.OrdinalIgnoreCase)))
            .ToListAsync();
    }

    public async Task<PhotoMetadata[]> GetTopLatestPhotos(int number)
    {
        return await context.Photos.OrderByDescending(p => p.DateTaken).Take(number).ToArrayAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}