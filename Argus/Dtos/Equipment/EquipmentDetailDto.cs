using Argus.Enums;

namespace Argus.Dtos.Equipment
{
    public class EquipmentDetailDto
    {
        public Guid Id { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public EquipmentType Type { get; set; }
        public EquipmentStatus Status { get; set; } 
        public string? IpAddress { get; set; }
    }
}
