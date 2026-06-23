namespace Argus.Dtos.Auditoriums
{
    public class CreateAuditoriumDto
    {
        public required string RoomNumber { get; set; } = string.Empty;
        public required int BuildingNumber { get; set; }
    }
}
