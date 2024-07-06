using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Room8.API.Dtos;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Data.Context;
using Room8.Core.Utilities;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room8.API.Services
{
   
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
           
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(UserManager<User> userManager, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
				_userManager = userManager;
				_appDbContext = appDbContext;
           

				_httpContextAccessor = httpContextAccessor;
        }
		

        public async Task<ResponseDto<EditProfileResponseDto>> EditProfile(EditProfileDto editProfileDto)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                var errors = new List<Error>
                {
                    new Error("404", "User does not exist")
                };
                return ResponseDto<EditProfileResponseDto>.Failure(errors, 404);
            }

			user.PhoneNumber = editProfileDto.PhoneNumber;
			user.Location = editProfileDto.Location;
			user.Occupation = editProfileDto.Occupation;
			user.Gender = editProfileDto.Sex;
			user.ZodiacSign = editProfileDto.Zodiac;

			var updateResult = await _userManager.UpdateAsync(user);
			if (!updateResult.Succeeded)
			{
				return ResponseDto<EditProfileResponseDto>.Failure(updateResult.Errors.Select(e => new Error(e.Code, e.Description)));
			}

			var editProfileResponseDto = new EditProfileResponseDto
			{
				UserId = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Location = user.Location,
				Occupation = user.Occupation
			};

            return ResponseDto<EditProfileResponseDto>.Success("Profile updated successfully", 200);
        }

        public async Task<ResponseDto<PaginatorDto<IEnumerable<UserResponseDto>>>> GetUsers(int pageNumber, int pageSize)
        {
            var users = _userManager.Users.Select(MapToUserResponse).Paginate(new PaginationFilter(pageNumber, pageSize));
            return ResponseDto<PaginatorDto<IEnumerable<UserResponseDto>>>.Success(users, "Successfully gotten Users", 200);
        }

        public UserResponseDto MapToUserResponse(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Age = user.Age,
                Gender = user.Gender ?? "",
                CreatedAt = user.CreatedAt,
                ApartmentName = user.Apartments.Count > 0 ? user.Apartments[0].Name : "",
            };
        }
    
			
	

		public async Task<ResponseDto<UserDto>> GetUserById(string id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				var errors = new List<Error>
				{
					new Error("404", "User does not exist")
				};
				return ResponseDto<UserDto>.Failure(errors, 404);
			}
			var userDto = new UserDto
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Location = user.Location,
				Occupation = user.Occupation
				

			};

			return ResponseDto<UserDto>.Success(userDto, "User retrieved successfully", 200);
		}
	}
}

