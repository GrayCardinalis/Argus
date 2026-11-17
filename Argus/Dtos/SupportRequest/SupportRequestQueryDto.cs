using Argus.Enums;

namespace Argus.Dtos.SupportRequest
{
    public class SupportRequestQueryDto
    {
        public SupportRequestStatus? Status { get; set; }
        public Guid? ExecutorId { get; set; }
        public Guid? AuditoriumId { get; set; }
        public DateTimeOffset? CreatedFrom { get; set; }
        public DateTimeOffset? CreatedTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
