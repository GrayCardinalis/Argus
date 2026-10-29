using Argus.Dtos.Auditoriums;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Argus.Constants.RouteNames;
using Argus.Enums;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Argus.Dtos.Equipment;

namespace Argus.Controllers
{
    [Route("api/[controller]")]
    public class AuditoriumController(IAuditoriumService auditoriumService, IEquipmentService equipmentService) : ApiController
    {
        [HttpGet(Name = AuditoriumRoutes.GetAllAuditoriums)]
        public async Task<ActionResult<List<AuditoriumDto>>> GetAllAuditoriums(CancellationToken ct)
        {
            var auditoriums = await auditoriumService.GetAllAuditoriumsAsync(ct);
            return Ok(auditoriums);
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
            var deleted = await auditoriumService.DeleteAuditoriumAsync(id, ct);
            return deleted 
                ? NoContent() 
                : NotFound($"Auditorium with ID: {id} not found");
        }

        [HttpGet("{id:guid}/equipment", Name = AuditoriumRoutes.GetEquipmentByAuditorium)]
        public async Task<ActionResult<List<EquipmentListDto>>> GetEquipmentByAuditoriumAsync(Guid id, CancellationToken ct)
        {
            var equipments = await equipmentService.GetEquipmentByAuditoriumAsync(id, ct);

            return equipments.Match<ActionResult<List<EquipmentListDto>>>(
                equipmentList => Ok(equipmentList),
                errors => Problem(errors));
        }
    }
}
