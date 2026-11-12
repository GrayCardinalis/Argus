using Argus.Constants.Database;
using Argus.Constants.Errors;
using Argus.Data;
using Argus.Dtos.SupportRequest;
using Argus.Enums;
using Argus.Enums.EquipmentStatuses;
using Argus.Models;
using Argus.Providers.Interfaces;
using Argus.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Argus.Services
{
    public class SupportRequestService(
        AppDbContext context,
        IMapper mapper,
        ICurrentUserProvider currentUser) : ISupportRequestService
    {
        public async Task<ErrorOr<SupportRequestDetailDto>> CreateSupportRequestAsync(CreateSupportRequestDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid clientId)
                return UserErrors.Unauthorized;

            var clientExists = await context.Users
                .AnyAsync(u => u.Id == clientId, ct);

            if (!clientExists)
                return UserErrors.Unauthorized;

            var equipment = await context.Equipments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == dto.EquipmentId, ct);

            if (equipment == null)
                return EquipmentErrors.NotFound;

            if (equipment.Status == EquipmentStatus.Recycled)
                return SupportRequestErrors.SupportRequestEquipmentRecycled;

            var currentPlacement = await context.PlacementHistories
                .AsNoTracking()
                .Where(ph => ph.EquipmentId == equipment.Id
                    && ph.RemovedAt == null)
                .SingleOrDefaultAsync(ct);

            if (currentPlacement == null)
                return PlacementHistoryErrors.EquipmentNotPlaced;

            var newSupportRequest = mapper.Map<SupportRequest>(dto);

            newSupportRequest.ClientId = clientId;
            newSupportRequest.AuditoriumId = currentPlacement.AuditoriumId;

            // Status, CreatedAt, ExecutorId и ResolvedAt
            // получают начальные значения из модели SupportRequest.

            context.SupportRequests.Add(newSupportRequest);

            await context.SaveChangesAsync(ct);

            var result = await context.SupportRequests
                .AsNoTracking()
                .IgnoreQueryFilters([QueryFilterNames.UserSoftDelete])
                .Where(sr => sr.Id == newSupportRequest.Id)
                .ProjectTo<SupportRequestDetailDto>(mapper.ConfigurationProvider)
                .SingleAsync(ct);

            return result;
        }

        public async Task<ErrorOr<SupportRequestDetailDto>> GetSupportRequestByIdAsync(Guid supportRequestId, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, ct);

            if (user == null)
                return UserErrors.Unauthorized;

            var query = context.SupportRequests
                .AsNoTracking()
                .IgnoreQueryFilters([QueryFilterNames.UserSoftDelete])
                .Where(sr => sr.Id == supportRequestId);

            if (user.Role == UserRole.Client)
            {
                query = query.Where(sr => sr.ClientId == userId);
            }
            else if (user.Role != UserRole.Technician
                && user.Role != UserRole.Admin)
            {
                return UserErrors.Forbidden;
            }

            var supportRequest = await query
                .ProjectTo<SupportRequestDetailDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            return supportRequest;
        }
    }
}