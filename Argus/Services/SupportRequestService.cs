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
using Argus.Dtos.Common;
using Argus.Dtos.SupportRequest.Comments;
using Argus.Dtos.SupportRequest.Components;
using Argus.Extensions;
using System.Data;

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

            return await context.SupportRequests
                .AsNoTracking()
                .IgnoreQueryFilters([QueryFilterNames.UserSoftDelete])
                .Where(sr => sr.Id == newSupportRequest.Id)
                .ProjectTo<SupportRequestDetailDto>(mapper.ConfigurationProvider)
                .SingleAsync(ct);
        }

        public async Task<ErrorOr<SupportRequestDetailDto>> GetSupportRequestByIdAsync(Guid supportRequestId, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var userRole = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => (UserRole?)u.Role)
                .FirstOrDefaultAsync(ct);

            if (userRole == null)
                return UserErrors.Unauthorized;

            var supportRequest = await context.SupportRequests
                .IgnoreQueryFilters([QueryFilterNames.UserSoftDelete])
                .Where(sr => sr.Id == supportRequestId)
                .ProjectTo<SupportRequestDetailDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (userRole != UserRole.Admin && userRole != UserRole.Technician && supportRequest.Client.Id != userId)
                return UserErrors.Forbidden;

            return supportRequest;
        }
        public async Task<ErrorOr<Success>> AssignExecutorAsync(Guid supportRequestId, AssignExecutorDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => new {u.Role})
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
                .Select(u => new 
                { 
                    u.Id, 
                    u.Role 
                })
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

        public async Task<ErrorOr<Success>> UpdateSupportRequestStatusAsync(Guid supportRequestId,UpdateSupportRequestStatusDto dto,CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var userRole = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => (UserRole?)u.Role)
                .FirstOrDefaultAsync(ct);

            if (userRole == null)
                return UserErrors.Unauthorized;

            if (userRole != UserRole.Admin && userRole != UserRole.Technician)
                return SupportRequestErrors.StatusForbidden;

            var supportRequest = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId)
                .Select(sr => new 
                { 
                    sr.Status, 
                    sr.ExecutorId 
                })
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (userRole == UserRole.Technician && supportRequest.ExecutorId != userId)
                return SupportRequestErrors.StatusForbidden;

            if (supportRequest.Status == SupportRequestStatus.Resolved)
                return SupportRequestErrors.ResolvedRequestCannotBeUpdated;

            if (supportRequest.Status == dto.NewStatus)
                return Result.Success;

            if (supportRequest.ExecutorId == null)
                return SupportRequestErrors.ExecutorRequired;

            DateTimeOffset? resolvedAt =
                dto.NewStatus == SupportRequestStatus.Resolved
                    ? DateTimeOffset.UtcNow
                    : null;

            var updatedRows = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId
                    && sr.Status == supportRequest.Status
                    && sr.ExecutorId == supportRequest.ExecutorId)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(sr => sr.Status, dto.NewStatus)
                        .SetProperty(sr => sr.ResolvedAt, resolvedAt),
                    ct);

            if (updatedRows == 0)
                return SupportRequestErrors.StatusConflict;

            return Result.Success;
        }

        public async Task<ErrorOr<PagedResultDto<SupportRequestListDto>>> GetSupportRequestsAsync(SupportRequestQueryDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var userRole = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => (UserRole?)u.Role)
                .FirstOrDefaultAsync(ct);

            if (userRole == null)
                return UserErrors.Unauthorized;

            var query = context.SupportRequests.AsQueryable();

            if (userRole != UserRole.Admin && userRole != UserRole.Technician)
                query = query.Where(sr => sr.ClientId == userId);

            if (dto.ClientId is Guid clientId)
                query = query.Where(sr => sr.ClientId == clientId);

            if (dto.Status is SupportRequestStatus status)
                query = query.Where(sr => sr.Status == status);

            if (dto.ExecutorId is Guid executorId)
                query = query.Where(sr => sr.ExecutorId == executorId);

            if (dto.AuditoriumId is Guid auditoriumId)
                query = query.Where(sr => sr.AuditoriumId == auditoriumId);

            if (dto.CreatedFrom is DateTimeOffset createdFrom)
            {
                var fromUtc = createdFrom.ToUniversalTime();
                query = query.Where(sr => sr.CreatedAt >= fromUtc);
            }

            if (dto.CreatedTo is DateTimeOffset createdTo)
            {
                var toUtc = createdTo.ToUniversalTime();
                query = query.Where(sr => sr.CreatedAt < toUtc);
            }

            return await query
                .OrderByDescending(sr => sr.CreatedAt)
                .ThenByDescending(sr => sr.Id)
                .ToPagedResultAsync<SupportRequest, SupportRequestListDto>(dto, mapper.ConfigurationProvider, ct);
        }

        public async Task<ErrorOr<SupportRequestCommentDto>> AddCommentAsync(Guid supportRequestId,CreateSupportRequestCommentDto dto,CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Role,
                    u.FullName
                })
                .FirstOrDefaultAsync(ct);

            if (user == null)
                return UserErrors.Unauthorized;

            var supportRequest = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId)
                .Select(sr => new { sr.ClientId })
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (user.Role != UserRole.Admin && user.Role != UserRole.Technician && supportRequest.ClientId != userId)
                return UserErrors.Forbidden;

            var comment = new SupportRequestComment
            {
                SupportRequestId = supportRequestId,
                AuthorId = userId,
                Message = dto.Message.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.SupportRequestComments.Add(comment);
            await context.SaveChangesAsync(ct);

            return new SupportRequestCommentDto
            {
                Id = comment.Id,
                SupportRequestId = comment.SupportRequestId,
                AuthorName = user.FullName,
                Message = comment.Message,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<ErrorOr<PagedResultDto<SupportRequestCommentDto>>> GetCommentsAsync(Guid supportRequestId, PaginationQueryDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var userRole = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => (UserRole?)u.Role)
                .FirstOrDefaultAsync(ct);

            if (userRole == null)
                return UserErrors.Unauthorized;

            var supportRequest = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId)
                .Select(sr => new { sr.ClientId })
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (userRole != UserRole.Admin && userRole != UserRole.Technician && supportRequest.ClientId != userId)
                return UserErrors.Forbidden;

            return await context.SupportRequestComments
                .IgnoreQueryFilters([QueryFilterNames.UserSoftDelete])
                .Where(comment => comment.SupportRequestId == supportRequestId)
                .OrderBy(comment => comment.CreatedAt)
                .ThenBy(comment => comment.Id)
                .ToPagedResultAsync<SupportRequestComment, SupportRequestCommentDto>(dto, mapper.ConfigurationProvider, ct);
        }
        public async Task<ErrorOr<Success>> AddComponentAsync(Guid supportRequestId, AddSupportRequestComponentDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var userRole = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => (UserRole?)u.Role)
                .FirstOrDefaultAsync(ct);

            if (userRole == null)
                return UserErrors.Unauthorized;

            if (userRole != UserRole.Admin && userRole != UserRole.Technician)
                return UserErrors.Forbidden;

            await using var transaction = await context.Database
                .BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);

            // Блокируем строку заявки до завершения транзакции.
            var supportRequest = await context.SupportRequests
                .FromSqlInterpolated(
                    $"SELECT * FROM support_requests WHERE id = {supportRequestId} FOR UPDATE")
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (userRole == UserRole.Technician && supportRequest.ExecutorId != userId)
                return UserErrors.Forbidden;

            if (supportRequest.Status == SupportRequestStatus.Resolved)
                return SupportRequestErrors.ResolvedRequestCannotBeUpdated;

            var consumption = await context.SupportRequestComponent
                .FirstOrDefaultAsync(
                    src => src.SupportRequestId == supportRequestId && src.ComponentId == dto.ComponentId, ct);

            if (consumption != null && (long)consumption.Quantity + dto.Quantity > int.MaxValue)
                return ComponentErrors.ConsumptionQuantityLimitExceeded;

            // Проверяем остаток и уменьшаем его одной SQL-командой.
            var updatedRows = await context.Components
                .Where(c => c.Id == dto.ComponentId && c.Quantity >= dto.Quantity)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        c => c.Quantity,
                        c => c.Quantity - dto.Quantity), ct);

            if (updatedRows == 0)
            {
                var componentExists = await context.Components
                    .AnyAsync(c => c.Id == dto.ComponentId, ct);

                if (!componentExists)
                    return ComponentErrors.NotFound;

                return ComponentErrors.InsufficientStock;
            }

            if (consumption == null)
            {
                consumption = new SupportRequestComponent
                {
                    SupportRequestId = supportRequestId,
                    ComponentId = dto.ComponentId,
                    Quantity = dto.Quantity
                };

                context.SupportRequestComponent.Add(consumption);
            }
            else
            {
                consumption.Quantity += dto.Quantity;
            }

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success;
        }

        public async Task<ErrorOr<PagedResultDto<SupportRequestComponentDto>>> GetComponentsAsync(Guid supportRequestId, PaginationQueryDto dto, CancellationToken ct = default)
        {
            if (currentUser.UserId is not Guid userId)
                return UserErrors.Unauthorized;

            var userRole = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => (UserRole?)u.Role)
                .FirstOrDefaultAsync(ct);

            if (userRole == null)
                return UserErrors.Unauthorized;

            var supportRequest = await context.SupportRequests
                .Where(sr => sr.Id == supportRequestId)
                .Select(sr => new { sr.ClientId })
                .FirstOrDefaultAsync(ct);

            if (supportRequest == null)
                return SupportRequestErrors.SupportRequestNotFound;

            if (userRole != UserRole.Admin && userRole != UserRole.Technician && supportRequest.ClientId != userId)
                return UserErrors.Forbidden;

            return await context.SupportRequestComponent
                .Where(src => src.SupportRequestId == supportRequestId)
                .OrderBy(src => src.Component!.Name)
                .ThenBy(src => src.ComponentId)
                .ToPagedResultAsync<SupportRequestComponent, SupportRequestComponentDto>(
                    dto, mapper.ConfigurationProvider, ct);
        }
    }
}