using Argus.Enums.EquipmentStatuses;

namespace Argus.Dtos.Equipment
{
    public class UpdateEquipmentStatusDto
    {
        public required EquipmentOperationalStatus Status { get; set; }
    }
}
