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
    public class SupportRequestService(AppDbContext context,IMapper mapper,ICurrentUserProvider currentUser) : ISupportRequestService
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
            else if (user.Role != UserRole.Technician && user.Role != UserRole.Admin)
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
        public async Task<ErrorOr<Success>> AssignExecutorAsync(Guid supportRequestId, AssignExecutorDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.Role })
                .FirstOrDefaultAsync(ct);

            if (user == null)
                return UserErrors.Unauthorized;

            if (user.Role != UserRole.Admin && user.Role != UserRole.Technician)
                return SupportRequestErrors.AssignmentForbidden;

            if (user.Role == UserRole.Technician && dto.ExecutorId != userId)
                return SupportRequestErrors.AssignmentForbidden;

            var supportRequest = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId)
                .Select(sr => new {sr.Status, sr.ExecutorId })
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (supportRequest.Status == SupportRequestStatus.Resolved)
                return SupportRequestErrors.ResolvedRequestCannotBeReassigned;

            if (user.Role == UserRole.Technician && supportRequest.ExecutorId is Guid assignedExecutorId && assignedExecutorId != userId)
                return SupportRequestErrors.AssignmentForbidden;

            var executor = await context.Users
                .Where(u => u.Id == dto.ExecutorId)
                .Select(u => new { u.Id, u.Role })
                .FirstOrDefaultAsync(ct);

            if (executor == null)
                return UserErrors.NotFound;

            if (executor.Role != UserRole.Technician)
                return SupportRequestErrors.ExecutorMustBeTechnician;

            if (supportRequest.ExecutorId == executor.Id)
                return Result.Success;

            var previousExecutorId = supportRequest.ExecutorId;

            var updateRows = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId && sr.Status != SupportRequestStatus.Resolved && sr.ExecutorId == previousExecutorId)
                .ExecuteUpdateAsync(
                setter => setter.SetProperty(
                    sr => sr.ExecutorId,
                    (Guid?)executor.Id), 
                ct);

            if (updateRows == 0)
                return SupportRequestErrors.AssignmentConflict;

            return Result.Success;
        }

        public async Task<ErrorOr<Success>> ChangeStatusAsync(Guid supportRequestId, AssignExecutorDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            return Result.Success;
        }
    }
}