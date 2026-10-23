using FluentValidation;
using Argus.Dtos.PlacementHistory;

namespace Argus.Validators.PlacementHistory
{
    public class MoveEquipmentDtoValidator : AbstractValidator<MoveEquipmentDto>
    {
        public MoveEquipmentDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(e => e.NewLocationId)
                .NotEmpty();
        }
    }
}
