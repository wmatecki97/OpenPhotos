using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.Database;

public interface IOpenPhotosDbContext
{
    DbSet<PhotoMetadata> Photos { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}