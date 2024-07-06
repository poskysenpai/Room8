using System.ComponentModel.DataAnnotations;

namespace Room8.API.Dtos
{
    public class EditProfileDto
    {
        public string PhoneNumber { get; set; } = "";
        public string Location { get; set; } = "";
        public string Occupation { get; set; } = "";
        public string Sex { get; set; } = "";
        public string Zodiac { get; set; } = "";


    }
}
