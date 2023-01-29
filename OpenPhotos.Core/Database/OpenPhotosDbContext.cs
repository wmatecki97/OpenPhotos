using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.Database
{
    internal class OpenPhotosDbContext : DbContext, IOpenPhotosDbContext
    {
        public DbSet<PhotoMetadata> Photos { get; set; }

        public OpenPhotosDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
