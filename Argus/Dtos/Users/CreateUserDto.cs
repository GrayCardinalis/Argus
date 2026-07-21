using Argus.Enums;

namespace Argus.Dtos.Users
{
    public class CreateUserDto
    {
        public required string FullName { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string UserName { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string ConfirmPassword { get; set; } = string.Empty;
        public required UserRole Role { get; set; }
    }
}