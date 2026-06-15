using Argus.Enums;

namespace Argus.Models
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public EquipmentType Type { get; set; }

        public EquipmentStatus Status { get; set; }
        public string? IpAddress { get; set; }
    }
}
