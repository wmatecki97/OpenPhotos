using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces.Repositories;

namespace OpenPhotos.Core.Database.Repositories;

public class PhotosRepository : IPhotosRepository
{
    private readonly IOpenPhotosDbContext _context;

    public PhotosRepository(IOpenPhotosDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task Add(PhotoMetadata photo)
    {
        await _context.Photos.AddAsync(photo);
    }

    public async Task<List<PhotoMetadata>> GetAll()
    {
        return await _context.Photos.ToListAsync();
    }

    public async Task<PhotoMetadata> GetByName(string name)
    {
        return await _context.Photos.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<List<PhotoMetadata>> GetByTag(string tag)
    {
        return await _context.Photos.Include(p => p.Tags)
            .Where(p => p.Tags.Any(t => t.Value.Contains(tag, StringComparison.OrdinalIgnoreCase)))
            .ToListAsync();
    }

    public async Task<PhotoMetadata[]> GetTopLatestPhotos(int number)
    {
        return await _context.Photos.OrderByDescending(p => p.DateTaken).Take(number).ToArrayAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}