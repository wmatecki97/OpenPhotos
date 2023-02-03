using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Web.Dtos;
using OpenPhotos.Web.Interfaces;

namespace OpenPhotos.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotosController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IPhotosBusinessLogic photosLogic;

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

    [HttpGet("Image/{imageName}")]
    public IActionResult GetImage(string imageName)
    {
        var imageBytes = photosLogic.GetImageBytes(imageName);
        var extension = Path.GetExtension(imageName);
        return File(imageBytes, $"image/{extension}");
    }

    [HttpPost]
    public async Task<IActionResult> AddPhoto([FromForm] IFormFile photo)
    {
        using (var memoryStream = new MemoryStream())
        {
            await photo.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var uploadData = new PhotoUploadDto
            {
                Name = photo.FileName,
                PhotoBytes = fileBytes
            };
            await photosLogic.UploadPhoto(uploadData);
        }

        return Ok();
    }
}