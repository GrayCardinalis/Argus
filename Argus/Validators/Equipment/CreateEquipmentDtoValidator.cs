using FluentValidation;
using Argus.Dtos.Equipment;
using Argus.Constants.Database;

namespace Argus.Validators.Equipment
{
    public class CreateEquipmentDtoValidator : AbstractValidator<CreateEquipmentDto>
    {
        public CreateEquipmentDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(e => e.InventoryNumber)
                .NotEmpty()
                .MaximumLength(EquipmentFieldLengths.InventoryNumberMax);

            RuleFor(e => e.IpAddress)
                .MaximumLength(EquipmentFieldLengths.IpAddressMax);

            RuleFor(e => e.ModelName)
                .NotEmpty()
                .MaximumLength(EquipmentFieldLengths.ModelNameMax);
            
            RuleFor(e => e.Type)
                .IsInEnum();
        }
    }
}