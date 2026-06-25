namespace Argus.Models
{
    public class Auditorium
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RoomNumber { get; set; } = string.Empty;
        public int BuildingNumber { get; set; }
    }
}
