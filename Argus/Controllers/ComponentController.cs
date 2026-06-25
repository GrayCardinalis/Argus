using Argus.Dtos.Components;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController(IComponentService componentService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ComponentDto>>> GetAllComponentAsync()
        {
            return Ok(await componentService.GetAllComponentAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComponentDto?>> GetComponent(Guid id)
        {
            var component = await componentService.GetComponent(id);
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

            return CreatedAtAction(nameof(GetComponent), new {id = createdComponent.Id}, createdComponent);
        }

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult> UpdateComponentStockAsync(Guid id, [FromBody] UpdateComponentStockDto updateComponentStockDto)
        {
            var isUpdated = await componentService.UpdateComponentStockAsync(id, updateComponentStockDto);
            return isUpdated
                ? NoContent() //204 error (perfect for update operation)
                : NotFound($"Component with id {id} not found."); //404 error
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
