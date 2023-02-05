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
    private readonly IMapper _mapper;
    private readonly IPhotosBusinessLogic _photosLogic;

    public PhotosController(IPhotosBusinessLogic photosLogic, IMapper mapper)
    {
        _photosLogic = photosLogic;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTop50Async()
    {
        var photos = await _photosLogic.GetMostCurrentPhotosAsync(50);
        var dtos = _mapper.Map<PhotoMetadata[], PhotoMetadataDto[]>(photos);
        return Ok(dtos);
    }

    [HttpGet("Image/{imageName}")]
    public IActionResult GetImage(string imageName)
    {
        var imageBytes = _photosLogic.GetImageBytes(imageName);
        var extension = Path.GetExtension(imageName);
        return File(imageBytes, $"image/{extension}");
    }

    [HttpGet("RemoveInconsistencies")]
    public async Task<IActionResult> RemoveInconsistencies()
    {
        await _photosLogic.RemoveInconsistencies();
        return Ok();
    }

    [HttpGet("RegenerateAllThumbnails")]
    public async Task<IActionResult> RegenerateAllThumbnails()
    {
        await _photosLogic.RegenerateAllThumbnails();
        return Ok();
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
            await _photosLogic.UploadPhoto(uploadData);
        }

        return Ok();
    }
}