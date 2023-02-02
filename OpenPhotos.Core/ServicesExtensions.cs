using Microsoft.Extensions.DependencyInjection;
using OpenPhotos.Core.Database;
using Microsoft.EntityFrameworkCore;
using OpenPhotos.Core.Interfaces.Repositories;
using OpenPhotos.Core.Database.Repositories;
using OpenPhotos.Core.Interfaces;
using OpenPhotos.Core.FileProcessing;
using OpenPhotos.Core.FileSystem;
using OpenPhotos.Core.Messaging;

namespace OpenPhotos.Core
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddOpenPhotosCoreServices(this IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext< IOpenPhotosDbContext, OpenPhotosDbContext>(options =>
            {
                options.UseNpgsql("User ID=admin;Password=admin;Host=localhost;Port=5432;Database=OpenPhotos;");
            });

            services.AddScoped<IPhotosRepository, PhotosRepository>();
            services.AddScoped<IFileMetadataReader, FileMetadataReader>();
            services.AddScoped<IFileSystem, FtpFileSystem>();
            services.AddScoped<IMessagePublisher, RabbitPublisher>();

            return services;
        }
    }
}
