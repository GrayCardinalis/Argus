using Argus.Dtos.SupportRequest;
using Argus.Validators.Common;
using FluentValidation;

namespace Argus.Validators.SupportRequest
{
    public class SupportRequestQueryDtoValidator: AbstractValidator<SupportRequestQueryDto>
    {
        public SupportRequestQueryDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            Include(new PaginationQueryDtoValidator());

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status must be a valid SupportRequestStatus enum value.")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.ExecutorId)
                .NotEqual(Guid.Empty)
                .WithMessage("ExecutorId must not be an empty GUID.")
                .When(x => x.ExecutorId.HasValue);

            RuleFor(x => x.AuditoriumId)
                .NotEqual(Guid.Empty)
                .WithMessage("AuditoriumId must not be an empty GUID.")
                .When(x => x.AuditoriumId.HasValue);

            RuleFor(x => x.CreatedFrom)
                .LessThan(x => x.CreatedTo)
                .WithMessage("CreatedFrom must be less than CreatedTo.")
                .When(x => x.CreatedFrom.HasValue && x.CreatedTo.HasValue);

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("ClientId must not be an empty GUID.")
                .When(x => x.ClientId.HasValue);
        }
    }
}