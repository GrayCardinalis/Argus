using FluentValidation;
using Argus.Dtos.Authorization;
using Argus.Constants.Database;
using Argus.Constants.Security;

namespace Argus.Validators.Users
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.UserNameMax);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(AuthConstants.PasswordMinLength)
                .Must(password => System.Text.Encoding.UTF8.GetByteCount(password) <= AuthConstants.PasswordMaxBytes)
                .WithMessage($"Password must not exceed {AuthConstants.PasswordMaxBytes} bytes in UTF-8 encoding.");
        }
    }
}