using Argus.Constants.Database;
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
        public int Page { get; set; } = PaginationDefaults.Page;
        public int PageSize { get; set; } = PaginationDefaults.PageSize;
    }
}
