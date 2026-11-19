using Argus.Constants.RouteNames;
using Argus.Dtos.Common;
using Argus.Dtos.Components;
using Argus.Enums;
using Argus.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Controllers
{
    [Route("api/[controller]")]
    public class ComponentController(IComponentService componentService) : ApiController
    {

        [HttpGet(Name = ComponentRoutes.GetAllComponent)]
        public async Task<ActionResult<PagedResultDto<ComponentDto>>> GetAllComponentAsync([FromQuery] PaginationQueryDto dto, CancellationToken ct)
        {
            var result = await componentService.GetAllComponentAsync(dto, ct);
            return Ok(result);
        }

        [HttpGet("{id}", Name = ComponentRoutes.GetComponentById)]
        public async Task<ActionResult<ComponentDto>> GetComponentByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await componentService.GetComponentByIdAsync(id, ct);

            return result.Match<ActionResult<ComponentDto>>(
                component => Ok(component),
                errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost(Name = ComponentRoutes.CreateComponent)]
        public async Task<ActionResult<ComponentDto>> CreateComponentAsync([FromBody] CreateComponentDto dto, CancellationToken ct)
        {
            var result = await componentService.CreateComponentAsync(dto, ct);

            return result.Match<ActionResult<ComponentDto>>(
                component => CreatedAtRoute(ComponentRoutes.GetComponentById, new { id = component.Id }, component),
                errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id:guid}/name", Name = ComponentRoutes.UpdateComponentName)]
        public async Task<IActionResult> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto dto, CancellationToken ct)
        {
            var result = await componentService.UpdateComponentNameAsync(id, dto, ct);

            return result.Match<IActionResult>(
                success => NoContent(),
                errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id:guid}/stock", Name = ComponentRoutes.AdjustComponentStock)]
        public async Task<IActionResult> AdjustComponentStockAsync(Guid id, [FromBody] AdjustComponentStockDto dto, CancellationToken ct)
        {
            var result = await componentService.AdjustComponentStockAsync(id, dto, ct);

            return result.Match<IActionResult>(
                success => NoContent(),
                errors => Problem(errors));
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Technician)}")]
        [HttpPost("{id:guid}/receipts", Name = ComponentRoutes.ReceiveComponentStock)]
        public async Task<IActionResult> ReceiveComponentStockAsync(Guid id, [FromBody] ReceiveComponentStockDto dto, CancellationToken ct)
        {
            var result = await componentService.ReceiveComponentStockAsync(id, dto, ct);

            return result.Match<IActionResult>(
                success => NoContent(),
                errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}", Name = ComponentRoutes.DeleteComponent)]
        public async Task<IActionResult> DeleteComponent(Guid id, CancellationToken ct)
        {
            var result = await componentService.DeleteComponentAsync(id, ct);

            return result.Match(
                success => NoContent(),
                errors => Problem(errors)); 
        }
    }
}
