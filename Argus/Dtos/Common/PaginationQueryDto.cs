using Argus.Constants.Database;

namespace Argus.Dtos.Common
{
    public class PaginationQueryDto
    {
        public int Page { get; set; } = PaginationDefaults.Page;
        public int PageSize { get; set; } = PaginationDefaults.PageSize;
    }
}