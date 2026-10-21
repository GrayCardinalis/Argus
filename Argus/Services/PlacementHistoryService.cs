using Argus.Data;
using AutoMapper;
using Argus.Services.Interfaces;
using ErrorOr;
using Argus.Dtos.PlacementHistory;
using Microsoft.EntityFrameworkCore;
using Argus.Models;
using Argus.Constants.Errors;
using AutoMapper.QueryableExtensions;

namespace Argus.Services
{
    public class PlacementHistoryService(AppDbContext context, IMapper mapper) : IPlacementHistoryService
    {
        public async Task<ErrorOr<List<PlacementHistoryDto>>> GetEquipmentHistoryAsync(Guid equipmentId, CancellationToken ct = default)
        {
            var equipmentExists = await context.Equipments.AnyAsync(e => e.Id == equipmentId, ct);

            if (!equipmentExists)
                return PlacementHistoryErrors.EquipmentNotFound;

            var historyEquipment = await context.PlacementHistories
                .AsNoTracking()
                .Where(ph => ph.EquipmentId == equipmentId)
                .OrderByDescending(ph => ph.InstalledAt)
                .ProjectTo<PlacementHistoryDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return historyEquipment;
        }

        public async Task<ErrorOr<Success>> MoveEquipmentAsync(Guid equipmentId, MoveEquipmentDto dto, CancellationToken ct = default)
        {
            var equipmentExists = await context.Equipments.AnyAsync(e => e.Id == equipmentId, ct);

            if (!equipmentExists)
                return PlacementHistoryErrors.EquipmentNotFound;

            var auditoriumExists = await context.Auditoriums.AnyAsync(a => a.Id == dto.NewLocationId, ct);

            if (!auditoriumExists)
                return PlacementHistoryErrors.AuditoriumNotFound;

            var currentPlacement = await context.PlacementHistories
                .Where(ph => ph.EquipmentId == equipmentId && ph.RemovedAt == null)
                .SingleOrDefaultAsync(ct);

            if(currentPlacement!=null && currentPlacement.AuditoriumId == dto.NewLocationId)
                return PlacementHistoryErrors.AlreadyInLocation;

            if (currentPlacement != null)
                currentPlacement.RemovedAt = DateTimeOffset.UtcNow;

            var newPlacementHistory = new PlacementHistory
            {
                EquipmentId = equipmentId,
                AuditoriumId = dto.NewLocationId
            };

            context.PlacementHistories.Add(newPlacementHistory);

            await context.SaveChangesAsync(ct);

            return Result.Success;
        }
    }
}
