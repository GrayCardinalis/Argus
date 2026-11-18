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
            return result.Match<ActionResult<PagedResultDto<ComponentDto>>>(page => Ok(page), errors => Problem(errors));
        }

        [HttpGet("{id}", Name = ComponentRoutes.GetComponentById)]
        public async Task<ActionResult<ComponentDto>> GetComponentByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await componentService.GetComponentByIdAsync(id, ct);
            return result.Match<ActionResult<ComponentDto>>(item => Ok(item), errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost(Name = ComponentRoutes.CreateComponent)]
        public async Task<ActionResult<ComponentDto>> CreateComponentAsync(CreateComponentDto createComponentDto, CancellationToken ct)
        {
            var result = await componentService.CreateComponentAsync(createComponentDto, ct);
            return result.Match<ActionResult<ComponentDto>>(
                item => CreatedAtRoute(ComponentRoutes.GetComponentById, new { id = item.Id }, item),
                errors => Problem(errors));
        }

        // SCENARIO 1: Full editing of the product card (Admin)
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id:guid}/name", Name = ComponentRoutes.UpdateComponentName)]
        public async Task<IActionResult> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto dto, CancellationToken ct)
        {
            var result = await componentService.UpdateComponentNameAsync(id, dto, ct);
            return result.Match<IActionResult>(_ => NoContent(), errors => Problem(errors));
        }

        // SCENARIO 2: Rapid change in inventory levels
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Technician))]
        [HttpPut("{id:guid}/stock", Name = ComponentRoutes.UpdateComponentStock)]
        public async Task<IActionResult> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto dto, CancellationToken ct)
        {
            var result = await componentService.UpdateComponentStockAsync(id, dto, ct);
            return result.Match<IActionResult>(_ => NoContent(), errors => Problem(errors));
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
