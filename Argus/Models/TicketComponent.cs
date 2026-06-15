namespace Argus.Models
{
    public class TicketComponent
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; } // The ID of the repair ticket this component belongs to
        public required RepairTicket Ticket { get; set; }
        public Guid ComponentId { get; set; } // The ID of the component that needs to be repaired or replaced
        public required Component Component { get; set; }
        public int Quantity { get; set; } // The quantity of the component needed for the repair

    }
}
