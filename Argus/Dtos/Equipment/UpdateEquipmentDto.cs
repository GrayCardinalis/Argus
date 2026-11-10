using Argus.Enums.EquipmentStatuses;

namespace Argus.Dtos.Equipment
{
    public class UpdateEquipmentDto
    {
        public required string ModelName { get; set; }
        public required string? IpAddress { get; set; }
    }
}
