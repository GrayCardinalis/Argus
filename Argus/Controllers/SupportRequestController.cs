using Argus.Constants.RouteNames;
using Argus.Dtos.SupportRequest;
using Argus.Enums;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Argus.Dtos.Common;
using Argus.Dtos.SupportRequest.Comments;
using Argus.Dtos.SupportRequest.Components;

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

        [HttpGet(Name = SupportRequestRoutes.GetSupportRequests)]
        public async Task<IActionResult> GetSupportRequestsAsync([FromQuery] SupportRequestQueryDto dto,CancellationToken ct)
        {
            var result = await supportRequestService.GetSupportRequestsAsync(dto, ct);

            return result.Match(
                response => Ok(response),
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

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Technician)}")]
        [HttpPost("{id:guid}/assign-executor", Name = SupportRequestRoutes.AssignExecutor)]
        public async Task<IActionResult> AssignExecutorAsync(Guid Id, AssignExecutorDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.AssignExecutorAsync(Id, dto, ct);
            
            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Technician)}")]
        [HttpPatch("{id:guid}/update-status", Name = SupportRequestRoutes.UpdateSupportRequestStatus)]
        public async Task<IActionResult> UpdateSupportRequestStatusAsync(Guid id, UpdateSupportRequestStatusDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.UpdateSupportRequestStatusAsync(id, dto, ct);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("{id:guid}/comments", Name = SupportRequestRoutes.AddComment)]
        public async Task<IActionResult> AddCommentAsync(Guid id, [FromBody] CreateSupportRequestCommentDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.AddCommentAsync(id, dto, ct);

            return result.Match(
                comment => StatusCode(StatusCodes.Status201Created, comment),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}/comments", Name = SupportRequestRoutes.GetComments)]
        public async Task<IActionResult> GetCommentsAsync(Guid id, [FromQuery] PaginationQueryDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.GetCommentsAsync(id, dto, ct);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Technician)}")]
        [HttpPost("{id:guid}/components", Name = SupportRequestRoutes.AddComponent)]
        public async Task<IActionResult> AddComponentAsync(Guid id, [FromBody] AddSupportRequestComponentDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.AddComponentAsync(id, dto, ct);

            return result.Match(
                success => NoContent(),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}/components", Name = SupportRequestRoutes.GetComponents)]
        public async Task<ActionResult<PagedResultDto<SupportRequestComponentDto>>> GetComponentsAsync(Guid id, [FromQuery] PaginationQueryDto dto, CancellationToken ct)
        {
            var result = await supportRequestService.GetComponentsAsync(id, dto, ct);

            return result.Match<ActionResult<PagedResultDto<SupportRequestComponentDto>>>(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
