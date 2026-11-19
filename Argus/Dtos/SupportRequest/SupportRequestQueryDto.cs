using Argus.Dtos.Common;
using Argus.Enums;

namespace Argus.Dtos.SupportRequest
{
    public class SupportRequestQueryDto : PaginationQueryDto
    {
        public SupportRequestStatus? Status { get; set; }
        public Guid? ExecutorId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? AuditoriumId { get; set; }
        public DateTimeOffset? CreatedFrom { get; set; }
        public DateTimeOffset? CreatedTo { get; set; }
    }
}