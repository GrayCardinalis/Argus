namespace Argus.Models
{
    public class SupportRequestComment
    {
        public int Id { get; set; }
        public Guid SupportRequestId { get; set; } // The ID of the repair ticket this comment belongs to
        public Guid AuthorId { get; set; } // The ID of the user who made the comment
        public SupportRequest? SupportRequest { get; set; }
        public User? Author { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow; // The current date and time in UTC by default when creating an object
    }
}
