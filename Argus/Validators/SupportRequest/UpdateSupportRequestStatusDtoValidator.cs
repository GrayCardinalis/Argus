using Argus.Dtos.SupportRequest;
using Argus.Enums;
using FluentValidation;

namespace Argus.Validators.SupportRequest
{
    public class UpdateSupportRequestStatusDtoValidator : AbstractValidator<UpdateSupportRequestStatusDto>
    {
        public UpdateSupportRequestStatusDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.NewStatus)
                .IsInEnum();
        }
    }
}
