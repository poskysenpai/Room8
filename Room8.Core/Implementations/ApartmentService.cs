using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Core.Utilities;
using Room8.Data.Context;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;
using Room8.Infrastructure.Helpers;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Room8.Core.Implementations
{
    public class ApartmentService : IApartmentService
    {
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<SavedApartment> _savedApartmentRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _appContext;
        private readonly IImageService _imageService;



        public ApartmentService(IRepository<Apartment> apartmentRepository, IImageService imageService, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, AppDbContext appContext, IRepository<SavedApartment> savedApartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _appContext = appContext;
            _imageService = imageService;
            _savedApartmentRepository = savedApartmentRepository;
        }
        public async Task<ResponseDto<ApartmentResponseDto>> GetApartmentById(long apartmentId)
        {
            try
            {
                var apartment = await _appContext.Apartments
                                                 .Include(a => a.ApartmentImages)
                                                 .Include(a => a.User)
                                                 .FirstOrDefaultAsync(x => x.Id == apartmentId);

                if (apartment == null)
                {
                    var errors = new List<Dtos.Error> { new Dtos.Error("404", "Apartment not found") };
                    return ResponseDto<ApartmentResponseDto>.Failure(errors, 404);
                }

                var apartmentResponseDto = new ApartmentResponseDto
                {
                    Id = apartment.Id,
                    Name = apartment.Name,
                    Description = apartment.Description,
                    Address = apartment.Address,
                    Location = apartment.Location,
                    VideoUrl = apartment.VideoUrl,
                    Price = apartment.Price,
                    NumberOfRooms = apartment.NumberOfRooms,
                    Features = apartment.Features,
                    CategoryName = apartment.Category?.Name,
                    IsAvailable = apartment.IsAvailable,
                    OwnerId = apartment.UserId,
                    OwnerEmail = apartment.User.Email,
                    OwnerName = $"{apartment.User?.FirstName} {apartment.User?.LastName}",
                    OwnerPhone = apartment.User.PhoneNumber,
                    IsSaved = apartment.IsSaved,
                    CreatedAt = apartment.CreatedAt,
                    UpdatedAt = apartment.UpdatedAt,
                    IsDeleted = apartment.IsDeleted,
                    ImageUrls = apartment.ApartmentImages.Select(x => x.ImageUrl).ToList()
                };

                return ResponseDto<ApartmentResponseDto>.Success(apartmentResponseDto, "Apartment with ID: {apartmentId} successfully retrieved", 200);
            }
            catch (Exception ex)
            {
                var errors = new List<Dtos.Error> { new Dtos.Error("500", ex.Message) };
                return ResponseDto<ApartmentResponseDto>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>> GetApartments(int pageNumber, int pageSize)
        {

            var apartments = await _appContext.Apartments.ToListAsync();
            var responses = new List<AllApartmentsResponseDto>();

            foreach(var a in apartments)
            {
                var response = await MapToApartmentResponse(a);
                responses.Add(response);
            }

            var res = responses.Paginate(new PaginationFilter(pageNumber, pageSize));
            //var apartments = _apartmentRepository.FindByCondition(x => true)
            //                .Include(x => x.Category)
            //                .Include(x => x.User)
            //                .Select(MapToApartmentResponse)
            //                .Paginate(new PaginationFilter(pageNumber, pageSize));

            return ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>.Success(res, "Successfully gotten Apartments", 200);
        }

        public async Task<ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>> GetSavedApartments(int pageNumber, int pageSize)
        {
            var errors = new List<Dtos.Error>();
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                errors.Add(new Dtos.Error("401", "User is not logged in."));
                return ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>.Failure(errors, 401);
            }

            var savedApartments = await _appContext.Apartments.Where(x => x.UserId == user.Id && x.IsSaved == true).ToListAsync();

            var responses = new List<AllApartmentsResponseDto>();

            foreach (var a in savedApartments)
            {
                var response = await MapToApartmentResponse(a);
                responses.Add(response);
            }

            var res = responses.Paginate(new PaginationFilter(pageNumber, pageSize));

            //var savedApartments = _apartmentRepository.FindByCondition(x => x.IsSaved)
            //                .Include(x => x.Category)
            //                .Include(x => x.User)
            //                .Select(MapToApartmentResponse)
            //                .Paginate(new PaginationFilter(pageNumber, pageSize));

            return ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>.Success(res, "Successfully gotten Saved Apartments", 200);
        }


        public async Task<ResponseDto<AptRequestDTO>> CreateApartment(AptRequestDTO RequestDTO, User user)
        {
            //var user = await _userManager.GetUserAsync(User);

            List<ApartmentImage> images = new List<ApartmentImage>();
            var errors = new List<Dtos.Error>();
            var error = new List<CloudinaryDotNet.Actions.Error>();
            

           

            var Name = new Category
            {
                Name = RequestDTO.TypeOfUnit,
            };


            var apartment = new Apartment
            {
                User = user,

                UserId = user.Id,
                Name = RequestDTO.Name,
                Address = RequestDTO.Address,
                Location = RequestDTO.Location,
                //ApartmentImages = images,// SET TO LIST OF APARTMENT IMAGES',
                Price = RequestDTO.Price,
                Features = RequestDTO.Features,
                Description = RequestDTO.Description,
                CategoryId = RequestDTO.CategoryId,
                NumberOfRooms = RequestDTO.NumberOfRooms,
                IsAvailable = true,
                IsDeleted = false

            };

            await _apartmentRepository.CreateAsync(apartment);

            foreach (var item in RequestDTO.ImageUrls)
            {
                //IFormFile IformImage;
                //try
                //{
                //     IformImage = IFormFileExtensions.ToIFormFile(item, $"{Guid.NewGuid()}.jpg");
                //}
                //catch (Exception ex)
                //{
                //    errors.Add(new Dtos.Error("500", "Error converting base64 string to file"));
                //    return ResponseDto<AptRequestDTO>.Failure(errors, 500);

                //}

                var imageUrl = await _imageService.UploadImageAsync(item);

                var imgResult = imageUrl.Data;
                var image = new ApartmentImage
                {
                    PublicId = imgResult.PublicId,
                    ImageUrl = imgResult.Url.ToString(),
                    ApartmentId = apartment.Id,
                    Apartment = apartment,
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsDeleted = false,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                images.Add(image);

                await _appContext.ApartmentImages.AddAsync(image);
                await _appContext.SaveChangesAsync();

                apartment.ApartmentImages.Add(image);

            }

             _appContext.Apartments.Update(apartment);

            return ResponseDto<AptRequestDTO>.Success("Successfully created new Apartment", 201);
        }


        public  async Task<AllApartmentsResponseDto> MapToApartmentResponse(Apartment apartment)
        {

            var image = await _appContext.ApartmentImages
                                   .Where(x => x.ApartmentId == apartment.Id)
                                   .Select(x => x.ImageUrl)
                                   .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(apartment.UserId);

            string name = $"{user?.FirstName} {user?.LastName}";

            return new AllApartmentsResponseDto
            {
                Id = apartment.Id,
                Name = apartment.Name,
                Description = apartment.Description,
                Address = apartment.Address,
                Location = apartment.Location,
                ImageUrl = image,
                VideoUrl = apartment.VideoUrl,
                Price = apartment.Price,
                NumberOfRooms = apartment.NumberOfRooms,
                Features = apartment.Features,
                CategoryName = apartment.Category?.Name,
                IsAvailable = apartment.IsAvailable,
                OwnerId = apartment.UserId,
                OwnerName = name,
                IsSaved = apartment.IsSaved,
                CreatedAt = apartment.CreatedAt,
                UpdatedAt = apartment.UpdatedAt,
                IsDeleted = apartment.IsDeleted
            };
        }

        public async Task<ResponseDto<PaginatorDto<List<PropertyByUserDto>>>> GetAllProperties()
        {
            try
            {
                var errors = new List<Dtos.Error>();

                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (user == null)
                {
                    errors.Add(new Dtos.Error("401", "User is not logged in."));
                    return ResponseDto<PaginatorDto<List<PropertyByUserDto>>>.Failure(errors, 401);
                }

                var allProperties = await _apartmentRepository.GetAllAsync();


                var userProperties = allProperties.Where(x => x.UserId == user.Id).ToList();

                var propertiesByUser = new List<PropertyByUserDto>();

                foreach(var u in userProperties)
                {
                    var selectedImage = await _appContext.ApartmentImages.FirstOrDefaultAsync(x => x.ApartmentId == u.Id);

                    var propertyByUser = new PropertyByUserDto
                    {
                        Id = u.Id,
                        Address = u.Address,
                        Name = u.Name,
                        ImageUrl = selectedImage is not null ? selectedImage.ImageUrl : null,
                        NoOfRooms = u.NumberOfRooms
                    };

                    propertiesByUser.Add(propertyByUser);
                }


                var paginatorDto = new PaginatorDto<List<PropertyByUserDto>>
                {
                    PageSize = propertiesByUser.Count,
                    CurrentPage = propertiesByUser.Count,
                    NumberOfPages = propertiesByUser.Count,
                    PageItems = propertiesByUser

                };

                return ResponseDto<PaginatorDto<List<PropertyByUserDto>>>.Success(paginatorDto,"Successfully fetched all properties", 200);
            }
            catch (Exception ex)
            {
                var errors = new List<Dtos.Error> { new Dtos.Error("500", ex.Message) };
                return ResponseDto<PaginatorDto<List<PropertyByUserDto>>>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<SavedApartmentResponse>> SaveApartment(long apartmentId)
        {
            try
            {
                var errors = new List<Dtos.Error>();

                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (user == null)
                {
                    errors.Add(new Dtos.Error("401", "User is not logged in."));
                    return ResponseDto<SavedApartmentResponse>.Failure(errors, 401);
                }

                var apartment = await _apartmentRepository.FindByCondition(x => x.Id == apartmentId).FirstOrDefaultAsync();

                if(apartment is null)
                {
                    errors.Add(new Dtos.Error("404", "Apartment not found"));
                    return ResponseDto<SavedApartmentResponse>.Failure(errors, 404);
                }

                var getSavedApartmentByUser = await _savedApartmentRepository.FindByCondition(x => x.ApartmentId == apartment.Id)
                                                                               .FirstOrDefaultAsync();

                if(getSavedApartmentByUser != null && getSavedApartmentByUser.UserId == user.Id)
                {
                    errors.Add(new Dtos.Error("403", "You've already saved this apartment"));
                    return ResponseDto<SavedApartmentResponse>.Failure(errors, 403);
                }

                var savedApartment = new SavedApartment
                {
                    SavedDate = DateTime.UtcNow,
                    ApartmentId = apartment.Id,
                    Apartment = apartment,
                    UserId = user.Id,
                    User = user,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    IsDeleted = false

                };

                await _savedApartmentRepository.CreateAsync(savedApartment);

                apartment.IsSaved = true;

                await _apartmentRepository.UpdateAsync(apartment);

                var response = new SavedApartmentResponse
                {
                    Id = savedApartment.Id,
                    ApartmentId = savedApartment.ApartmentId,
                    SavedDate = savedApartment.SavedDate,
                    UserId = savedApartment.UserId
                };

                return ResponseDto<SavedApartmentResponse>.Success(response, "Apartment saved successfully", 201);

            }
            catch(Exception ex)
            {
                var errors = new List<Dtos.Error> { new Dtos.Error("500", ex.Message) };
                return ResponseDto<SavedApartmentResponse>.Failure(errors, 500);
            }
        }
    }
}

