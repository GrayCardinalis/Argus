using Argus.Constants.Errors;
using Argus.Data;
using Argus.Dtos.Equipment;
using Argus.Enums.EquipmentStatuses;
using Argus.Models;
using Argus.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Argus.Dtos.Common;
using Argus.Extensions;

namespace Argus.Services
{
    public class EquipmentService(AppDbContext context, IMapper mapper) : IEquipmentService
    {
        public async Task<ErrorOr<PagedResultDto<EquipmentListDto>>> GetAllEquipmentAsync(PaginationQueryDto dto,CancellationToken ct = default)
        {
            return await context.Equipments
                .OrderBy(e => e.InventoryNumber)
                .ThenBy(e => e.Id)
                .ToPagedResultAsync<Equipment, EquipmentListDto>(dto,mapper.ConfigurationProvider,ct);
        }
        public async Task<ErrorOr<EquipmentDetailDto>> GetEquipmentByIdAsync(Guid id, CancellationToken ct = default)
        {
            var equipment = await context.Equipments
                .AsNoTracking()
                .Where(e=>e.Id == id)
                .ProjectTo<EquipmentDetailDto?>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (equipment == null)
                return EquipmentErrors.NotFound;

            return equipment;
        }
        public async Task<ErrorOr<PagedResultDto<EquipmentListDto>>> GetEquipmentByAuditoriumAsync(Guid auditoriumId,PaginationQueryDto dto,CancellationToken ct = default)
        {
            var auditoriumExists = await context.Auditoriums
                .AnyAsync(a => a.Id == auditoriumId, ct);

            if (!auditoriumExists)
                return EquipmentErrors.AuditoriumNotFound;

            return await context.PlacementHistories
                .Where(ph => ph.AuditoriumId == auditoriumId && ph.RemovedAt == null)
                .Select(ph => ph.Equipment!)
                .OrderBy(e => e.InventoryNumber)
                .ThenBy(e => e.Id)
                .ToPagedResultAsync<Equipment, EquipmentListDto>(dto, mapper.ConfigurationProvider, ct);
        }
        public async Task<ErrorOr<EquipmentDetailDto>> CreateEquipmentAsync(CreateEquipmentDto dto, CancellationToken ct = default)
        {
            var isInventoryNumberExists = await context.Equipments
                .AnyAsync(n => n.InventoryNumber == dto.InventoryNumber, ct);

            if(isInventoryNumberExists)
                return EquipmentErrors.DuplicateInventoryNumber;

            var newEquipment = mapper.Map<Equipment>(dto);
            newEquipment.Status = EquipmentStatus.Active;

            newEquipment.IpAddress = string.IsNullOrWhiteSpace(dto.IpAddress)
                ? null
                : dto.IpAddress.Trim();

            context.Equipments.Add(newEquipment);

            await context.SaveChangesAsync(ct);

            return mapper.Map<EquipmentDetailDto>(newEquipment);
        }

        public async Task<ErrorOr<Success>> UpdateEquipmentStatusAsync(Guid id, UpdateEquipmentStatusDto dto, CancellationToken ct = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var equipment = await context.GetEquipmentForUpdateAsync(id, ct);

            if (equipment == null)
                return EquipmentErrors.NotFound;

            if(equipment.Status == EquipmentStatus.Recycled)
                return EquipmentErrors.AlreadyRecycled;

            switch(dto.Status)
            {
                case EquipmentOperationalStatus.Active:
                    equipment.Status = EquipmentStatus.Active;
                    break;

                case EquipmentOperationalStatus.UnderRepair:
                    equipment.Status = EquipmentStatus.UnderRepair;
                    break;

                default:
                    return EquipmentErrors.InvalidOperationalStatus;
            }

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success;
        }
        public async Task<ErrorOr<Success>> UpdateEquipmentAsync(Guid id, UpdateEquipmentDto dto, CancellationToken ct = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var equipment = await context.GetEquipmentForUpdateAsync(id, ct);   

            if (equipment == null)
                return EquipmentErrors.NotFound;

            if(equipment.Status == EquipmentStatus.Recycled)
                return EquipmentErrors.AlreadyRecycled;

            mapper.Map(dto, equipment);

            equipment.IpAddress = string.IsNullOrWhiteSpace(dto.IpAddress)
                ? null
                : dto.IpAddress.Trim();

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success;
        }
        public async Task<ErrorOr<Success>> DeleteEquipmentAsync(Guid id, CancellationToken ct = default)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var equipment = await context.GetEquipmentForUpdateAsync(id, ct);

            if (equipment == null)
                return EquipmentErrors.NotFound;

            equipment.Status = EquipmentStatus.Recycled;

            var activePlacement = await context.PlacementHistories
                .Where(ph => ph.EquipmentId == id && ph.RemovedAt == null)
                .SingleOrDefaultAsync(ct);

            if (activePlacement != null)
                activePlacement.RemovedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success;
        }
    }
}
