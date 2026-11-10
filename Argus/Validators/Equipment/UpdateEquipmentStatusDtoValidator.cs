using FluentValidation;
using Argus.Dtos.Equipment;

namespace Argus.Validators.Equipment
{
    public class UpdateEquipmentStatusDtoValidator :AbstractValidator<UpdateEquipmentStatusDto>
    {
        public UpdateEquipmentStatusDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(e => e.Status)
                .IsInEnum();
        }
    }
}
