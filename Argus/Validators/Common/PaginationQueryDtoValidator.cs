using Argus.Constants.Database;
using Argus.Dtos.Common;
using FluentValidation;

namespace Argus.Validators.Common
{
    public class PaginationQueryDtoValidator: AbstractValidator<PaginationQueryDto>
    {
        public PaginationQueryDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Page)
                .GreaterThan(0);

            RuleFor(dto => dto.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(PaginationDefaults.MaxPageSize);
        }
    }
}