namespace Argus.Dtos.SupportRequest
{
    public class CreateSupportRequestDto
    {
        public required string Title { get; set; } = string.Empty; // Short title for the repair ticket
        public required string Description { get; set; } = string.Empty; // Detailed description of the issue
        public Guid EquipmentId { get; set; } 
        public Guid AuditoriumId { get; set; } 
    }
}
