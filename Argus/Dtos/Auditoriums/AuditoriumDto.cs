namespace Argus.Dtos.Auditoriums
{
    public class AuditoriumDto
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int BuildingNumber { get; set; }
    }
}
