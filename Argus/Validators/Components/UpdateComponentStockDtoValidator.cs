using FluentValidation;
using Argus.Dtos.Components;

namespace Argus.Validators.Components
{
    public class UpdateComponentStockDtoValidator : AbstractValidator<UpdateComponentStockDto>
    {
        public UpdateComponentStockDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(0);
        }
    }
}
