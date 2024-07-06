using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Room8.Core.Dtos;

namespace Room8.Core.Abstractions
{
    public interface IImageService
    {
        Task<ResponseDto<ImageUploadResult>> UploadImageAsync(IFormFile photo);

        Task<ResponseDto<DeletionResult>> DeleteImageAsync(string publicId);
    }
}
