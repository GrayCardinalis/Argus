using Argus.Dtos.SupportRequest.Components;
using FluentValidation;

namespace Argus.Validators.SupportRequest.Components
{
    public class AddSupportRequestComponentDtoValidator: AbstractValidator<AddSupportRequestComponentDto>
    {
        public AddSupportRequestComponentDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.ComponentId)
                .NotEmpty();

            RuleFor(dto => dto.Quantity)
                .GreaterThan(0);
        }
    }
}