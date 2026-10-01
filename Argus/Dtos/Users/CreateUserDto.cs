using Argus.Enums;

namespace Argus.Dtos.Users
{
    public class CreateUserDto
    {
        public required string FullName { get; set; }
        public string? Department { get; set; } = string.Empty;
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}