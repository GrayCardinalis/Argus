using FluentValidation;
using Argus.Dtos.Components;
using Argus.Constants.Database;

namespace Argus.Validators.Components
{
    public class CreateComponentDtoValidator : AbstractValidator<CreateComponentDto>
    {
        public CreateComponentDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(ComponentFieldLengths.NameMax);

            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(0);
        }
    }
}
