namespace Argus.Models
{
    public class SupportRequestComponent
    {
        public Guid SupportRequestId { get; set; } // The ID of the repair request this component belongs to
        public Guid ComponentId { get; set; } // The ID of the component that needs to be repaired or replaced
        public int Quantity { get; set; } // The quantity of the component needed for the repair
        public SupportRequest? SupportRequest { get; set; }
        public Component? Component { get; set; }
    }
}
