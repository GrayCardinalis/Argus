using Argus.Constants.Errors;
using Argus.Data;
using Argus.Dtos.Equipment;
using Argus.Enums;
using Argus.Models;
using Argus.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Argus.Services
{
    public class EquipmentService(AppDbContext context, IMapper mapper) : IEquipmentService
    {
        public async Task<ErrorOr<List<EquipmentListDto>>> GetAllEquipmentAsync(CancellationToken ct = default)
        {
            var equipments = await context.Equipments
                .AsNoTracking()
                .ProjectTo<EquipmentListDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);
                
            return equipments;
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
        public async Task<ErrorOr<Success>> UpdateEquipmentAsync(Guid id, UpdateEquipmentDto dto, CancellationToken ct = default)
        {
            var equipment = await context.Equipments.FirstOrDefaultAsync(e => e.Id == id, ct);

            if(equipment == null)
                return EquipmentErrors.NotFound;

            mapper.Map(dto, equipment);

            equipment.IpAddress = string.IsNullOrWhiteSpace(dto.IpAddress)
                ? null
                : dto.IpAddress.Trim();

            if (dto.Status == EquipmentStatus.Recycled)
            {
                var activePlacement = await context.PlacementHistories
                    .Where(ph => ph.EquipmentId == id && ph.RemovedAt == null)
                    .SingleOrDefaultAsync(ct);

                if (activePlacement != null)
                    activePlacement.RemovedAt = DateTimeOffset.UtcNow;            
            }

            await context.SaveChangesAsync(ct);

            return Result.Success;
        }
        public async Task<ErrorOr<Success>> DeleteEquipmentAsync(Guid id, CancellationToken ct = default)
        {
            var equipment = await context.Equipments.FirstOrDefaultAsync(e => e.Id == id, ct);

            if (equipment == null)
                return EquipmentErrors.NotFound;

            equipment.Status = EquipmentStatus.Recycled;

            var activePlacement = await context.PlacementHistories
                .Where(ph => ph.EquipmentId == id && ph.RemovedAt == null)
                .SingleOrDefaultAsync(ct);

            if (activePlacement != null)
                activePlacement.RemovedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync(ct);

            return Result.Success;
        }

        public async Task<ErrorOr<List<EquipmentListDto>>> GetEquipmentByAuditoriumAsync(Guid auditoriumId, CancellationToken ct = default)
        {
            var auditorium = await context.Auditoriums.AnyAsync(a => a.Id == auditoriumId, ct);
            
            if (!auditorium)
                return EquipmentErrors.AuditoriumNotFound;

            var equipments = await context.PlacementHistories
                .AsNoTracking()
                .Where(ph => ph.AuditoriumId == auditoriumId && ph.RemovedAt == null)
                .Select(ph=>ph.Equipment!)
                .ProjectTo<EquipmentListDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return equipments;
        }
    }
}
