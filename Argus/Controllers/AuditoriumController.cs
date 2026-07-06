using Argus.Dtos.Auditoriums;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Argus.Constants.RouteNames;

namespace Argus.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriumController(IAuditoriumService auditoriumService) : ControllerBase
    {
        [HttpGet(Name = AuditoriumRoutes.GetAllAuditoriums)]
        public async Task<ActionResult<List<AuditoriumDto>>> GetAllAuditoriums()
        {
            var auditoriums = await auditoriumService.GetAllAuditoriumsAsync();
            return Ok(auditoriums);
        }

        [HttpGet("{id}", Name = AuditoriumRoutes.GetAuditoriumById)]
        public async Task<ActionResult<AuditoriumDto>> GetAuditoriumByIdAsync(Guid id)
        {
            var auditorium = await auditoriumService.GetAuditoriumByIdAsync(id);
            return auditorium is null 
                ? NotFound($"Auditorium with ID: {id} not found")
                : Ok(auditorium);
        }

        [HttpPost]
        public async Task<ActionResult<AuditoriumDto>> CreateAuditoriumAsync([FromBody] CreateAuditoriumDto createdAuditoriumDto)
        {
            var createdAuditorium = await auditoriumService.CreateAuditoriumAsync(createdAuditoriumDto);
            return CreatedAtRoute(AuditoriumRoutes.GetAuditoriumById, new { id = createdAuditorium.Id }, createdAuditorium);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuditorium(Guid id)
        {
            var deleted = await auditoriumService.DeleteAuditoriumAsync(id);
            return deleted 
                ? NoContent() 
                : NotFound($"Auditorium with ID: {id} not found");
        }
    }
}
