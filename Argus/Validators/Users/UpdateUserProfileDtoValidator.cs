using Argus.Constants.Database;
using Argus.Dtos.Users;
using FluentValidation;

namespace Argus.Validators.Users
{
    public class UpdateUserProfileDtoValidator : AbstractValidator<UpdateUserProfileDto>
    {
        public UpdateUserProfileDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.FullNameMax);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.UserNameMax);

            RuleFor(x => x.Department)
                .MaximumLength(UserFieldLengths.DepartmentMax);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(UserFieldLengths.EmailMax)
                .EmailAddress();
        }
    }
}