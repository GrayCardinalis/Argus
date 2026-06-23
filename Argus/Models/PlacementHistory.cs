namespace Argus.Models
{
    public class PlacementHistory
    {
        public int Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid AuditoriumId { get; set; }
        public Equipment? Equipment { get; set; }
        public Auditorium? Auditorium { get; set; }
        //DateTimeOffset.UtcNow; The current date and time in UTC by default when creating an object
        public DateTimeOffset InstalledAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? RemovedAt { get; set; }
    }
}
