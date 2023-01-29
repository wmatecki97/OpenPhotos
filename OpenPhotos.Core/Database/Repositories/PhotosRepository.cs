using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.Database.Repositories
{
    internal class PhotosRepository
    {
        private readonly IOpenPhotosDbContext dbContext;

        public PhotosRepository(IOpenPhotosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<PhotoMetadata>> GetAll()
        {
            return dbContext.Photos.ToListAsync();
        }

        public Task<PhotoMetadata> GetByName(string name)
        {
            return dbContext.Photos.FirstOrDefaultAsync(p => p.Name == name);
        }

        public Task<List<PhotoMetadata>> GetByTag(string tag)
        {
            return dbContext.Photos.Include(p => p.Tags)
                .Where(p => p.Tags.Any(t => t.Value.Contains(tag, StringComparison.OrdinalIgnoreCase)))
                .ToListAsync();
        }
    }
}
