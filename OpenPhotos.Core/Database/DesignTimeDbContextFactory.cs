using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace OpenPhotos.Core.Database
{
    /// <summary>
    /// Needed for running EF commands without calling ServicesExtensions that register dbcontext in the DI from external project
    /// </summary>
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OpenPhotosDbContext>
    {
        public OpenPhotosDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OpenPhotosDbContext>();
            {
                builder.UseNpgsql(Configuration.GetConnectionString());
                return new OpenPhotosDbContext(builder.Options);
            }
        }
    }
}
