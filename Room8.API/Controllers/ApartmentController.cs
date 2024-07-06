using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Domain.Entities;


namespace Room8.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ApartmentController : ControllerBase
    {
        private readonly IApartmentService _apartmentService;
        private readonly UserManager<User> _userManager;
        public ApartmentController(IApartmentService apartmentService, UserManager<User> userManager)
        {
            _apartmentService = apartmentService;
            _userManager = userManager;
        }


        [HttpGet("apartment-by-id/{apartmentId}")]
        public async Task<IActionResult> GetApartmentById(long apartmentId)
        {
            var result = await _apartmentService.GetApartmentById(apartmentId);
            if (result.IsSuccessful)
                return Ok(result);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("all-apartments")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = _apartmentService.GetApartments(pageNumber, pageSize).Result;
            return Ok(response);
        }

        [HttpGet("all-properties-by-user")]
        public async Task<IActionResult> GetAllProperties()
        {
            var result = await _apartmentService.GetAllProperties();

            if (result.IsSuccessful) return Ok(result);
          
              return StatusCode(result.StatusCode, result);
            
        }

        [HttpGet("saved")]
        public async Task<IActionResult> GetSavedApartments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _apartmentService.GetSavedApartments(pageNumber, pageSize);
            return Ok(response);
        }

        [HttpPost("Create")]
        
        public async Task<IActionResult> Create([FromForm] AptRequestDTO dTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var response = await _apartmentService.CreateApartment(dTO, user);
                return Ok(response);

            }
            return BadRequest();


        }

        [HttpPost("save-apartment/{apartmentId}")]
        public async Task<IActionResult> Save(long apartmentId)
        {
            var result = await _apartmentService.SaveApartment(apartmentId);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);

        }


    }
		
	
}
        

       
    

