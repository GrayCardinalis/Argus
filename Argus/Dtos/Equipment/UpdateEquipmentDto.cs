using Argus.Enums;

namespace Argus.Dtos.Equipment
{
    public class UpdateEquipmentDto
    {
        public required string ModelName { get; set; }
        public string? IpAddress { get; set; }
        public required EquipmentStatus Status { get; set; }
    }
}
