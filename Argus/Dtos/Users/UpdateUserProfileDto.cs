using Argus.Enums;

namespace Argus.Dtos.Users
{
    public class UpdateUserProfileDto
    {
        public required string FullName { get; set; }
        public required string UserName { get; set;}
        public string? Department { get; set; }
        public required string Email { get; set;}
    }
}
