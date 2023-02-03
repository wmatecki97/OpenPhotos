using AutoMapper;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Web.Dtos;

namespace OpenPhotos.Web;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PhotoMetadata, PhotoMetadataDto>();
    }
}