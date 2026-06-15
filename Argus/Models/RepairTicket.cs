namespace Argus.Models
{
    public class RepairTicket
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty; // Short title for the repair ticket
        public string Description { get; set; } = string.Empty; // Detailed description of the issue
        public required TicketComponent Status { get; set; } // Status of the repair ticket
        public enum TicketStatus
        {
            New = 1,
            InWork = 2,
            PendingComponents = 3,
            Resolved = 4
        }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow; // Timestamp when the ticket was created
        public DateTimeOffset? ResolvedAt { get; set; } // Timestamp when the ticket was resolved, nullable if not resolved yet
        public Guid ClientId { get; set; } // Foreign key to the Client who reported the issue
        public required User Client { get; set; }
        public Guid? ExecutorId { get; set; } // Foreign key to the User assigned to resolve the issue, nullable if not assigned yet
        public User? Executor { get; set; }
        public Guid EquipmentId { get; set; } // Foreign key to the Equipment that is being repaired
        public required Equipment Equipment { get; set; }
        public Guid AuditoriumId { get; set; } // Foreign key to the Auditorium where the equipment is located
        public required Auditorium Auditorium { get; set; }
    }
}
