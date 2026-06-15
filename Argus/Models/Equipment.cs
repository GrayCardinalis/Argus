namespace Argus.Models
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public EquipmentType Type { get; set; }
        public enum EquipmentType
        {
            Computer = 1,
            Printer = 2,
            Other = 3
        }
        public EquipmentType Status { get; set; }
        public enum EquipmentStatus
        {
            Active = 1,
            UnderRepair = 2,
            Recycled = 3
        }
        public string? ipAddress { get; set; }
    }
}
