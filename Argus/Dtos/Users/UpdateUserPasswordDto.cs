namespace Argus.Dtos.Users
{
    public class UpdateUserPasswordDto
    {
        public required string CurrentPassword { get; set; } = string.Empty;
        public required string NewPassword { get; set; } = string.Empty;
        public required string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
