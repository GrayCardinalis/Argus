using Argus.Dtos;
using Argus.Dtos.Auditoriums;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriumController(IAuditoriumService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AuditoriumDto>>> GetAllAuditoriums()
        {
            return Ok(await service.GetAllAuditoriumsAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditoriumDto>> GetAuditorium(Guid id)
        {
            var auditorium = await service.GetAuditoriumByIdAsync(id);
            return auditorium is null 
                ? NotFound($"Auditorium with ID: {id} not found")
                : Ok(auditorium);
        }

        [HttpPost]
        public async Task<ActionResult<AuditoriumDto>> CreateAuditorium([FromBody] CreateAuditoriumDto auditorium)
        {
            var createdAuditorium = await service.CreateAuditoriumAsync(auditorium);
            return CreatedAtAction(nameof(GetAuditorium), new { id = createdAuditorium.Id }, createdAuditorium);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuditorium(Guid id)
        {
            var deleted = await service.DeleteAuditoriumAsync(id);
            return deleted 
                ? NoContent() 
                : NotFound($"Auditorium with ID: {id} not found");
        }
    }
}
