using Argus.Enums;

namespace Argus.Dtos.Equipment
{
    public class EquipmentListDto
    {
        public Guid Id { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public EquipmentStatus Status { get; set; }
    }
}
