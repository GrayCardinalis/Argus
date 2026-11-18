using FluentValidation;
using Argus.Dtos.Components;

namespace Argus.Validators.Components
{
    public class UpdateComponentStockDtoValidator : AbstractValidator<UpdateComponentStockDto>
    {
        public UpdateComponentStockDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.ExpectedQuantity).GreaterThanOrEqualTo(0);
            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(0);
        }
    }
}
