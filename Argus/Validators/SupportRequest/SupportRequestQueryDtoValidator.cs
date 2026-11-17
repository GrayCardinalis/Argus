using Argus.Dtos.SupportRequest;
using FluentValidation;

namespace Argus.Validators.SupportRequest
{
    public class SupportRequestQueryDtoValidator: AbstractValidator<SupportRequestQueryDto>
    {
        public SupportRequestQueryDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

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

            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must be less than or equal to 100.");

            RuleFor(x => x.CreatedFrom)
                .LessThan(x => x.CreatedTo)
                .WithMessage("CreatedFrom must be less than CreatedTo.")
                .When(x => x.CreatedFrom.HasValue && x.CreatedTo.HasValue);
        }
    }
}