using Argus.Enums;

namespace Argus.Dtos.Equipment
{
    public class CreateEquipmentDto
    {
        public required string InventoryNumber { get; set; }
        public required string ModelName { get; set; }
        public required EquipmentType Type { get; set; }
        public string? IpAddress { get; set; }
    }
}
