using Room8.Core.Dtos;
using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Abstractions
{
    public interface IApartmentService
    {
        Task<ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>> GetApartments(int pageNumber, int pageSize);
        Task<ResponseDto<PaginatorDto<List<PropertyByUserDto>>>> GetAllProperties();
        Task<ResponseDto<PaginatorDto<IEnumerable<AllApartmentsResponseDto>>>> GetSavedApartments(int pageNumber, int pageSize);
        Task<ResponseDto<AptRequestDTO>> CreateApartment(AptRequestDTO RequestDTO, User user);
        Task<ResponseDto<ApartmentResponseDto>> GetApartmentById(long apartmentId);
        Task<ResponseDto<SavedApartmentResponse>> SaveApartment(long apartmentId);
    }
      
       
    

}


