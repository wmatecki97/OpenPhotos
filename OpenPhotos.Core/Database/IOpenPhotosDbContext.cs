using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Database.Entities;

namespace OpenPhotos.Core.Database
{
    internal interface IOpenPhotosDbContext
    {
        DbSet<PhotoMetadata> Photos { get; set; }
    }
}