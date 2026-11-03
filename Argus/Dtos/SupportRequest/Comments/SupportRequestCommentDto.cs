
namespace Argus.Dtos.SupportRequest.Comments
{
    public class SupportRequestCommentDto
    {
        public int Id { get; set; }
        public Guid SupportRequestId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
