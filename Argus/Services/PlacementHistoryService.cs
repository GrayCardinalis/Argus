using Argus.Data;
using AutoMapper;
using Argus.Services.Interfaces;
using ErrorOr;
using Argus.Dtos.PlacementHistory;
using Microsoft.EntityFrameworkCore;
using Argus.Models;
using Argus.Constants.Errors;
using AutoMapper.QueryableExtensions;
using Argus.Enums.EquipmentStatuses;
using Argus.Dtos.Common;
using Argus.Extensions;

namespace Argus.Services
{
    public class PlacementHistoryService(AppDbContext context, IMapper mapper) : IPlacementHistoryService
    {
        public async Task<ErrorOr<PagedResultDto<PlacementHistoryDto>>> GetEquipmentHistoryAsync(Guid equipmentId, PaginationQueryDto dto, CancellationToken ct = default)
        {
            var equipmentExists = await context.Equipments
                .AnyAsync(e => e.Id == equipmentId, ct);

            if (!equipmentExists)
                return PlacementHistoryErrors.EquipmentNotFound;

            return await context.PlacementHistories
                .Where(ph => ph.EquipmentId == equipmentId)
                .OrderByDescending(ph => ph.InstalledAt)
                .ThenByDescending(ph => ph.Id)
                .ToPagedResultAsync<PlacementHistory, PlacementHistoryDto>(dto, mapper.ConfigurationProvider, ct);
        }

        public async Task<ErrorOr<Success>> MoveEquipmentAsync(Guid equipmentId, MoveEquipmentDto dto, CancellationToken ct = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var equipment = await context.GetEquipmentForUpdateAsync(equipmentId, ct);

            if (equipment is null)
                return PlacementHistoryErrors.EquipmentNotFound;

            if (equipment.Status == EquipmentStatus.Recycled)
                return PlacementHistoryErrors.EquipmentRecycled;

            var auditoriumExists = await context.Auditoriums
                .AnyAsync(a => a.Id == dto.NewLocationId, ct);

            if (!auditoriumExists)
                return PlacementHistoryErrors.AuditoriumNotFound;

            var currentPlacement = await context.PlacementHistories
                .SingleOrDefaultAsync(ph => ph.EquipmentId == equipmentId && ph.RemovedAt == null, ct);

            if (currentPlacement?.AuditoriumId == dto.NewLocationId)
                return PlacementHistoryErrors.AlreadyInLocation;

            var movedAt = DateTimeOffset.UtcNow;

            if (currentPlacement is not null)
            {
                currentPlacement.RemovedAt = movedAt;
                await context.SaveChangesAsync(ct);
            }

            var newPlacement = new PlacementHistory
            {
                EquipmentId = equipmentId,
                AuditoriumId = dto.NewLocationId,
                InstalledAt = movedAt
            };

            context.PlacementHistories.Add(newPlacement);

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success;
        }
    }
}
