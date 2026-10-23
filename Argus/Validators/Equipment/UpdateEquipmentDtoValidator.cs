using FluentValidation;
using Argus.Dtos.Equipment;
using Argus.Constants.Database;

namespace Argus.Validators.Equipment
{
    public class UpdateEquipmentDtoValidator : AbstractValidator<UpdateEquipmentDto>
    {
        public UpdateEquipmentDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(e => e.ModelName)
                .NotEmpty()
                .MaximumLength(EquipmentFieldLengths.ModelNameMax);

            RuleFor(e => e.IpAddress)
                .MaximumLength(EquipmentFieldLengths.IpAddressMax);

            RuleFor(e => e.Status)
                .IsInEnum();
        }
    }
}
