namespace Argus.Dtos.PlacementHistory
{
    public class PlacementHistoryDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public DateTimeOffset InstalledAt { get; set; }
        public DateTimeOffset? RemovedAt { get; set; }
    }
}
