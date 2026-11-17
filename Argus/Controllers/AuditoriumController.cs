using Argus.Constants.RouteNames;
using Argus.Dtos.Auditoriums;
using Argus.Dtos.Common;
using Argus.Dtos.Equipment;
using Argus.Enums;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Controllers
{
    [Route("api/[controller]")]
    public class AuditoriumController(IAuditoriumService auditoriumService, IEquipmentService equipmentService) : ApiController
    {
        [HttpGet(Name = AuditoriumRoutes.GetAllAuditoriums)]
        public async Task<ActionResult<PagedResultDto<AuditoriumDto>>> GetAllAuditoriums([FromQuery] PaginationQueryDto dto, CancellationToken ct)
        {
            var result = await auditoriumService.GetAllAuditoriumsAsync(dto, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}/equipment", Name = AuditoriumRoutes.GetEquipmentByAuditorium)]
        public async Task<ActionResult<PagedResultDto<EquipmentListDto>>> GetEquipmentByAuditoriumAsync(Guid id, [FromQuery] PaginationQueryDto dto, CancellationToken ct)
        {
            var result = await equipmentService.GetEquipmentByAuditoriumAsync(id, dto, ct);

            return result.Match<ActionResult<PagedResultDto<EquipmentListDto>>>(
                response => Ok(response),
                errors => Problem(errors));
        }

        [HttpGet("{id}", Name = AuditoriumRoutes.GetAuditoriumById)]
        public async Task<ActionResult<AuditoriumDto>> GetAuditoriumByIdAsync(Guid id, CancellationToken ct)
        {
            var auditorium = await auditoriumService.GetAuditoriumByIdAsync(id, ct);
            return auditorium is null 
                ? NotFound($"Auditorium with ID: {id} not found")
                : Ok(auditorium);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost(Name = AuditoriumRoutes.CreateAuditorium)]
        public async Task<ActionResult<AuditoriumDto>> CreateAuditoriumAsync(CreateAuditoriumDto createdAuditoriumDto, CancellationToken ct)
        {
            var createdAuditorium = await auditoriumService.CreateAuditoriumAsync(createdAuditoriumDto, ct);
            return CreatedAtRoute(AuditoriumRoutes.GetAuditoriumById, new { id = createdAuditorium.Id }, createdAuditorium);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}", Name = AuditoriumRoutes.DeleteAuditorium)]
        public async Task<IActionResult> DeleteAuditorium(Guid id, CancellationToken ct)
        {
            var result = await auditoriumService.DeleteAuditoriumAsync(id, ct);
            return result.Match(
                success => NoContent(),
                errors => Problem(errors));
        }
    }
}
