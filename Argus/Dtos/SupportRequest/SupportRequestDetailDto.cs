using Argus.Dtos.Users;
using Argus.Enums;

namespace Argus.Dtos.SupportRequest
{
    public class SupportRequestDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SupportRequestStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ResolvedAt { get; set; }

        //Other Dto
        public UserDto Client { get; set; } = null!;

    }
}
