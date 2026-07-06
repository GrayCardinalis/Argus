using Argus.Enums;

namespace Argus.Dtos.Users
{
    public class UpdateUserProfileDto
    {
        public required string FullName { get; set; } = string.Empty;
        public required string UserName { get; set;} = string.Empty;
        public string? Department { get; set; }
        public required string Email { get; set;} = string.Empty;
        public required UserRole Role { get; set; }
    }
}
