using Argus.Dtos.Users;

namespace Argus.Dtos.Authorization
{
    public class LoginResponseDto
    {
        public required string AccessToken { get; set; } = string.Empty;
        public required UserDto User { get; set; } = new UserDto();
    }
}
