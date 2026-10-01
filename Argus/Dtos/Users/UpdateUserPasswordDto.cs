namespace Argus.Dtos.Users
{
    public class UpdateUserPasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; }
    }
}
