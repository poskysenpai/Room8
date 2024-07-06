using Room8.API.Dtos;
using Room8.Core.Dtos;
using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Abstractions
{
    public interface IUserService
    {
        Task<ResponseDto<EditProfileResponseDto>> EditProfile(EditProfileDto editProfileDto);
        Task<ResponseDto<PaginatorDto<IEnumerable<UserResponseDto>>>> GetUsers(int pageNumber, int pageSize);

        Task<ResponseDto<UserDto>> GetUserById(string id);
    }
		
	
}
