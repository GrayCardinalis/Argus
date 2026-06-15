namespace Argus.Models
{
    public class PlacementHistory
    {
        public int Id { get; set; }
        public Guid EquipmentId { get; set; }
        public required Equipment Equipment { get; set; }
        public Guid AuditoriumId { get; set; }
        public required Auditorium Auditorium { get; set; }
        //DateTimeOffset.UtcNow; The current date and time in UTC by default when creating an object
        public DateTimeOffset ExtractedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LeftAt { get; set; }
    }
}
