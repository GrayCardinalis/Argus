using FluentValidation;
using Argus.Dtos.Components;

namespace Argus.Validators.Components
{
    public class AdjustComponentStockDtoValidator : AbstractValidator<AdjustComponentStockDto>
    {
        public AdjustComponentStockDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.ExpectedQuantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.ActualQuantity)
                .GreaterThanOrEqualTo(0);
        }
    }
}
