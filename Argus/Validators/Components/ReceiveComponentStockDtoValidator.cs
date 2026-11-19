using Argus.Dtos.Components;
using FluentValidation;

namespace Argus.Validators.Components
{
    public class ReceiveComponentStockDtoValidator : AbstractValidator<ReceiveComponentStockDto>
    {
        public ReceiveComponentStockDtoValidator()
        {
            RuleFor(dto => dto.Quantity)
                .GreaterThan(0);
        }
    }
}