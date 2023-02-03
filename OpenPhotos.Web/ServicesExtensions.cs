using OpenPhotos.Web.BusinessLogic;
using OpenPhotos.Web.Interfaces;

namespace OpenPhotos.Web;

public static class ServicesExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IPhotosBusinessLogic, PhotosBusinessLogic>();

        return services;
    }
}