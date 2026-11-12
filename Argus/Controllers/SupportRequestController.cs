using Argus.Constants.RouteNames;
using Argus.Dtos.SupportRequest;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Controllers
{
    [Route("api/[controller]")]
    public class SupportRequestController(ISupportRequestService supportRequestService) : ApiController
    {
        [HttpPost(Name = SupportRequestRoutes.CreateSupportRequest)]
        public async Task<ActionResult<SupportRequestDetailDto>> CreateSupportRequestAsync(CreateSupportRequestDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.CreateSupportRequestAsync(dto, ct);
            return result.Match<ActionResult<SupportRequestDetailDto>>(
                supportRequest => CreatedAtRoute(
                    SupportRequestRoutes.GetSupportRequestById,
                    new { id = supportRequest.Id }, supportRequest),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}", Name = SupportRequestRoutes.GetSupportRequestById)]
        public async Task<ActionResult<SupportRequestDetailDto>> GetSupportRequestByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await supportRequestService.GetSupportRequestByIdAsync(id, ct);
            return result.Match<ActionResult<SupportRequestDetailDto>>(
                supportRequest => Ok(supportRequest),
                errors => Problem(errors));
        }
    }
}
