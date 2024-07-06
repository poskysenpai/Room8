using System.ComponentModel.DataAnnotations;

namespace Room8.Core.Dtos
{
   
        public class RegistrationRequestDTO
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
            [Required]
            [DataType(DataType.Password)]
            [StringLength(15, MinimumLength = 7, ErrorMessage = "Minimum of 7 charaters")]
            public string Password { get; set; } = "";
            public string Firstname { get; set; } = "";
            public string Lastname { get; set; } = "";
            public string? Social { get; set; }

        }
    
}
