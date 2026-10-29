using Argus.Dtos.Components;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Argus.Constants.RouteNames;
using Microsoft.AspNetCore.Authorization;
using Argus.Enums;

namespace Argus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController(IComponentService componentService) : ControllerBase
    {
        [HttpGet(Name = ComponentRoutes.GetAllComponent)]
        public async Task<ActionResult<List<ComponentDto>>> GetAllComponentAsync(CancellationToken ct)
        {
            var components = await componentService.GetAllComponentAsync(ct);
            return Ok(components);
        }

        [HttpGet("{id}", Name = ComponentRoutes.GetComponentById)]
        public async Task<ActionResult<ComponentDto?>> GetComponentByIdAsync(Guid id, CancellationToken ct)
        {
            var component = await componentService.GetComponentByIdAsync(id, ct);
            return component is null
                ? NotFound($"Component with id {id} not found.")
                : Ok(component);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost(Name = ComponentRoutes.CreateComponent)]
        public async Task<ActionResult<ComponentDto>> CreateComponentAsync(CreateComponentDto createComponentDto, CancellationToken ct)
        {
            var createdComponent = await componentService.CreateComponentAsync(createComponentDto, ct);

            return CreatedAtRoute(ComponentRoutes.GetComponentById, new {id = createdComponent.Id}, createdComponent);
        }

        // SCENARIO 1: Full editing of the product card (Admin)
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id:guid}/name", Name = ComponentRoutes.UpdateComponentName)]
        public async Task<IActionResult> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto dto, CancellationToken ct)
        {
            var isUpdated = await componentService.UpdateComponentNameAsync(id, dto, ct);
            return isUpdated ? NoContent() : NotFound($"Component with id {id} not found.");
        }

        // SCENARIO 2: Rapid change in inventory levels
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Technician))]
        [HttpPut("{id:guid}/stock", Name = ComponentRoutes.UpdateComponentStock)]
        public async Task<IActionResult> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto dto, CancellationToken ct)
        {
            var isUpdated = await componentService.UpdateComponentStockAsync(id, dto, ct);
            return isUpdated ? NoContent() : NotFound($"Component with id {id} not found.");
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}", Name = ComponentRoutes.DeleteComponent)]
        public async Task<IActionResult> DeleteComponent(Guid id, CancellationToken ct)
        {
            var isDeleted = await componentService.DeleteComponentAsync(id, ct);

            return isDeleted
               ? NoContent() //204 error (perfect for delete operation)
               : NotFound($"Component with id {id} not found."); //404 error
        }
    }
}
