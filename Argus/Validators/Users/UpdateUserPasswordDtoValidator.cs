using FluentValidation;
using Argus.Dtos.Users;
using Argus.Constants.Security;

namespace Argus.Validators.Users
{
    public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(AuthConstants.PasswordMinLength)
                .Must(password => System.Text.Encoding.UTF8.GetByteCount(password) <= AuthConstants.PasswordMaxBytes)
                .WithMessage($"Current password must not exceed {AuthConstants.PasswordMaxBytes} bytes in UTF-8 encoding.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(AuthConstants.PasswordMinLength)
                .Must(password => System.Text.Encoding.UTF8.GetByteCount(password) <= AuthConstants.PasswordMaxBytes)
                .WithMessage($"New password must not exceed {AuthConstants.PasswordMaxBytes} bytes in UTF-8 encoding.");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage($"Passwords do not match.");
        }
    }
}
