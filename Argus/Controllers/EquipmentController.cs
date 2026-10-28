using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Argus.Dtos.Equipment;
using Argus.Constants.RouteNames;
using Microsoft.AspNetCore.Authorization;
using Argus.Enums;
using Argus.Dtos.PlacementHistory;

namespace Argus.Controllers
{
    [Route("api/[controller]")]
    public class EquipmentController(IEquipmentService equipmentService, IPlacementHistoryService placementHistoryService) : ApiController
    {
        [HttpGet(Name = EquipmentRoutes.GetAllEquipment)]
        public async Task<ActionResult<List<EquipmentListDto>>> GetAllEquipmentAsync(CancellationToken ct)
        {
            var result = await equipmentService.GetAllEquipmentAsync(ct);
            return result.Match<ActionResult<List<EquipmentListDto>>>(
                equipment => Ok(equipment),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}", Name = EquipmentRoutes.GetEquipmentById)]
        public async Task<ActionResult<EquipmentDetailDto>> GetEquipmentByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await equipmentService.GetEquipmentByIdAsync(id, ct);
            return result.Match<ActionResult<EquipmentDetailDto>>(
                equipment => Ok(equipment),
                errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost(Name = EquipmentRoutes.CreateEquipment)]
        public async Task<ActionResult<EquipmentDetailDto>> CreateEquipmentAsync(CreateEquipmentDto dto, CancellationToken ct)
        {
            var result = await equipmentService.CreateEquipmentAsync(dto, ct);
            return result.Match<ActionResult<EquipmentDetailDto>>(
                equipment => CreatedAtRoute(
                    EquipmentRoutes.GetEquipmentById,
                    new { id = equipment.Id }, equipment),
                errors => Problem(errors));
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Technician)}")]
        [HttpPatch("{id:guid}", Name = EquipmentRoutes.UpdateEquipment)]
        public async Task<IActionResult> UpdateEquipmentAsync(Guid id, UpdateEquipmentDto dto, CancellationToken ct)
        {
            var result = await equipmentService.UpdateEquipmentAsync(id, dto, ct);
            return result.Match(
                equipment => NoContent(),
                errors => Problem(errors));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id:guid}", Name = EquipmentRoutes.DeleteEquipment)]
        public async Task<IActionResult> DeleteEquipmentAsync(Guid id, CancellationToken ct)
        {
            var result = await equipmentService.DeleteEquipmentAsync(id, ct);
            return result.Match(
                equipment => NoContent(),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}/placement-history", Name = EquipmentRoutes.GetEquipmentHistory)]
        public async Task<ActionResult<List<PlacementHistoryDto>>> GetEquipmentHistoryAsync(Guid id, CancellationToken ct)
        {
            var result = await placementHistoryService.GetEquipmentHistoryAsync(id, ct);
            return result.Match<ActionResult<List<PlacementHistoryDto>>>(
                history => Ok(history),
                errors => Problem(errors));
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Technician)}")]
        [HttpPost("{id:guid}/move", Name = EquipmentRoutes.MoveEquipment)]
        public async Task<IActionResult> MoveEquipmentAsync(Guid id, MoveEquipmentDto dto, CancellationToken ct)
        {
            var result = await placementHistoryService.MoveEquipmentAsync(id, dto, ct);
            return result.Match(
                history => NoContent(),
                errors => Problem(errors));
        }
    }
}
