using System.Text;
using Argus.Constants.Database;
using Argus.Constants.Security;
using Argus.Dtos.Users;
using FluentValidation;

namespace Argus.Validators.Users
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.FullNameMax);

            RuleFor(x => x.Department)
                .MaximumLength(UserFieldLengths.DepartmentMax);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.EmailMax)
                .EmailAddress();

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.UserNameMax);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(AuthConstants.PasswordMinLength)
                .Must(password => Encoding.UTF8.GetByteCount(password) <= AuthConstants.PasswordMaxBytes)
                .WithMessage($"Password must not exceed {AuthConstants.PasswordMaxBytes} bytes in UTF-8 encoding.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");
        }
    }
}
