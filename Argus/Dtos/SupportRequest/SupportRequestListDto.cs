using Argus.Enums;

namespace Argus.Dtos.SupportRequest
{
    public class SupportRequestListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public SupportRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        // Flat mapping. We don't need the entire audience facility, just the room number.
        public string RoomNumber { get; set; } = string.Empty;
    }
}
