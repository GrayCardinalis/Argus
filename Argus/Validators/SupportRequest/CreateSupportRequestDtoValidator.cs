using Argus.Constants.Database;
using Argus.Dtos.SupportRequest;
using FluentValidation;

namespace Argus.Validators.SupportRequest
{
    public class CreateSupportRequestDtoValidator : AbstractValidator<CreateSupportRequestDto>
    {
        public CreateSupportRequestDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(sr => sr.Title)
                .NotEmpty()
                .MaximumLength(SupportRequestFieldLengths.TitleMax);

            RuleFor(sr => sr.Description)
                .NotEmpty()
                .MaximumLength(SupportRequestFieldLengths.DescriptionMax);
            
            RuleFor(sr => sr.EquipmentId)
                .NotEmpty();
        }
    }
}
