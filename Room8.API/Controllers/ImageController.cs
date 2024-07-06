using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Room8.Core.Abstractions;

namespace Room8.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile photo)
        {
            var apiResponse = await _imageService.UploadImageAsync(photo);
            return Ok(apiResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromQuery] string publicId)
        {
            var apiResponse = await _imageService.DeleteImageAsync(publicId);
            return Ok(apiResponse);
        }
    }
}
