using Microsoft.Extensions.DependencyInjection;
using OpenPhotos.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace OpenPhotos.Core
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddOpenPhotosCoreServices(this IServiceCollection services)
        {
            services.AddDbContext< IOpenPhotosDbContext, OpenPhotosDbContext>(options =>
            {
                options.UseNpgsql();
            });
            return services;
        }
    }
}
