using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;

namespace Room8.Core.Implementations
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary cloudinary;

        public ImageService(IConfiguration _configuration)
        {

            Account account = new Account
                (
                    _configuration["CloudinarySettings:Cloud"],
                    _configuration["CloudinarySettings:ApiKey"],
                    _configuration["CloudinarySettings:ApiSecret"]
                );
            cloudinary = new Cloudinary(account);
        }

        public async Task<ResponseDto<DeletionResult>> DeleteImageAsync(string publicId)
        {
            var errors = new List<Room8.Core.Dtos.Error>();
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var deleteResult = await cloudinary.DestroyAsync(deleteParams);
                return ResponseDto<DeletionResult>.Success(deleteResult, "Successfully deleted Image");
            } 
            catch (Exception ex)
            {
                errors.Add(new Room8.Core.Dtos.Error("500", "Unexpected error occured: " + ex.Message));
                return ResponseDto<DeletionResult>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<ImageUploadResult>> UploadImageAsync(IFormFile photo)
        {
            string[] allowedTypes = { "image/jpeg", "image/png", "image/jpg" };
            if (photo == null || photo.Length <= 0)
            {
                throw new ArgumentException("Image cannot be null or empty");
            }
            
            if (!allowedTypes.Contains(photo.ContentType))
            {
                throw new ArgumentException("Acceptable types are .png, .jpg and .jpeg") ;
            }
            ImageUploadResult imageUploadResult;
            try
            {
                using var fs = photo.OpenReadStream();
                imageUploadResult = await cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(photo.FileName, fs)
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Image upload failed: " + ex.Message);
            }
         
            return ResponseDto<ImageUploadResult>.Success(imageUploadResult, "Successfully Uploaded Image", 200);
        }
    }
}
