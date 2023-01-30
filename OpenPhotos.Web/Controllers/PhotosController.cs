using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces.Repositories;
using OpenPhotos.Web.Dtos;
using OpenPhotos.Web.Interfaces;

namespace OpenPhotos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotosBusinessLogic photosLogic;
        private readonly IMapper mapper;

        public PhotosController(IPhotosBusinessLogic photosLogic, IMapper mapper)
        {
            this.photosLogic = photosLogic;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTop50Async()
        {
            var photos = await photosLogic.GetMostCurrentPhotosAsync(50);
            var dtos = mapper.Map<PhotoMetadata[], PhotoMetadataDto[]>(photos);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(PhotoUploadDto photoDto)
        {
            await photosLogic.UploadPhoto(photoDto);
            return Ok();
        }
    }
}
