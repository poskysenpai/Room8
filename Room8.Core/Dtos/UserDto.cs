namespace Room8.Core.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Location { get; set; } = "";
        public string Occupation { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
