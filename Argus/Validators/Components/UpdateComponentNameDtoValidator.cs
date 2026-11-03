using FluentValidation;
using Argus.Dtos.Components;
using Argus.Constants.Database;

namespace Argus.Validators.Components
{
    public class UpdateComponentNameDtoValidator : AbstractValidator<UpdateComponentNameDto>
    {
        public UpdateComponentNameDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(ComponentFieldLengths.NameMax);
        }
    }
}
