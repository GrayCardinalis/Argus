using Argus.Dtos.Components;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Argus.Constants.RouteNames;

namespace Argus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController(IComponentService componentService) : ControllerBase
    {
        [HttpGet(Name = ComponentRoutes.GetAllComponent)]
        public async Task<ActionResult<List<ComponentDto>>> GetAllComponentAsync()
        {
            var components = await componentService.GetAllComponentAsync();
            return Ok(components);
        }

        [HttpGet("{id}", Name = ComponentRoutes.GetComponentById)]
        public async Task<ActionResult<ComponentDto?>> GetComponentByIdAsync(Guid id)
        {
            var component = await componentService.GetComponentByIdAsync(id);
            return component is null
                ? NotFound($"Component with id {id} not found.")
                : Ok(component);
            //Равно следующему коду:
            /*if (component is null)
                return NotFound($"Component with id {id} not found.");
                return Ok(component);*/
        }


        [HttpPost]
        public async Task<ActionResult<ComponentDto>> CreateComponentAsync([FromBody] CreateComponentDto createComponentDto)
        {
            var createdComponent = await componentService.CreateComponentAsync(createComponentDto);

            return CreatedAtRoute(ComponentRoutes.GetComponentById, new {id = createdComponent.Id}, createdComponent);
        }

        // СЦЕНАРИЙ 1: Полное редактирование карточки товара (Admin)
        [HttpPut("{id:guid}/name")]
        public async Task<IActionResult> UpdateComponentNameAsync(Guid id, [FromBody] UpdateComponentNameDto dto)
        {
            var isUpdated = await componentService.UpdateComponentNameAsync(id, dto);
            return isUpdated ? NoContent() : NotFound($"Component with id {id} not found.");
        }

        // СЦЕНАРИЙ 2: Быстрое изменение остатков на складе
        [HttpPut("{id:guid}/stock")]
        public async Task<IActionResult> UpdateComponentStockAsync(Guid id, [FromBody] UpdateComponentStockDto dto)
        {
            var isUpdated = await componentService.UpdateComponentStockAsync(id, dto);
            return isUpdated ? NoContent() : NotFound($"Component with id {id} not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComponent(Guid id)
        {
            var isDeleted = await componentService.DeleteComponentAsync(id);

            return isDeleted
               ? NoContent() //204 error (perfect for delete operation)
               : NotFound($"Component with id {id} not found."); //404 error
        }
    }
}
