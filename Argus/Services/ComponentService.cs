using Argus.Data;
using AutoMapper;
using Argus.Models;
using Argus.Dtos.Components;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Argus.Services.Interfaces;
using ErrorOr;
using Argus.Constants.Errors;
using Npgsql;
using Argus.Dtos.Common;
using Argus.Extensions;

namespace Argus.Services
{
    public class ComponentService(AppDbContext context, IMapper mapper) : IComponentService
    {
        public async Task<PagedResultDto<ComponentDto>> GetAllComponentAsync(PaginationQueryDto dto, CancellationToken ct = default)
        {
            return await context.Components
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .ToPagedResultAsync<Component, ComponentDto>(dto, mapper.ConfigurationProvider, ct);
        }

        public async Task<ErrorOr<ComponentDto>> GetComponentByIdAsync(Guid id, CancellationToken ct = default)
        {
            var component = await context.Components
                .Where(c => c.Id == id)
                .ProjectTo<ComponentDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (component is null)
                return ComponentErrors.NotFound;

            return component;
        }

        public async Task<ErrorOr<ComponentDto>> CreateComponentAsync(CreateComponentDto dto, CancellationToken ct = default)
        {
            var component = mapper.Map<Component>(dto);

            context.Components.Add(component);
            await context.SaveChangesAsync(ct);

            return mapper.Map<ComponentDto>(component);
        }
        public async Task<ErrorOr<Success>> AdjustComponentStockAsync(Guid id, AdjustComponentStockDto dto, CancellationToken ct = default)
        {
            var updatedRows = await context.Components
                .Where(c => c.Id == id && c.Quantity == dto.ExpectedQuantity)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        c => c.Quantity,
                        dto.ActualQuantity),ct);

            if (updatedRows > 0)
                return Result.Success;

            var componentExists = await context.Components
                .AnyAsync(c => c.Id == id, ct);

            if (!componentExists)
                return ComponentErrors.NotFound;

            return ComponentErrors.StockConflict;
        }

        public async Task<ErrorOr<Success>> ReceiveComponentStockAsync(Guid id, ReceiveComponentStockDto dto, CancellationToken ct = default)
        {
            var maxCurrentQuantity = int.MaxValue - dto.Quantity;

            var updatedRows = await context.Components
                .Where(c => c.Id == id && c.Quantity <= maxCurrentQuantity)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(c => c.Quantity, c => c.Quantity + dto.Quantity), ct);

            if (updatedRows > 0)
                return Result.Success;

            var componentExists = await context.Components
                .AnyAsync(c => c.Id == id, ct);

            if (!componentExists)
                return ComponentErrors.NotFound;

            return ComponentErrors.StockQuantityLimitExceeded;
        }

        public async Task<ErrorOr<Success>> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto dto, CancellationToken ct = default)
        {
            var component = await context.Components
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            if (component is null)
                return ComponentErrors.NotFound;

            mapper.Map(dto, component);
            await context.SaveChangesAsync(ct);

            return Result.Success;
        }

        public async Task<ErrorOr<Success>> DeleteComponentAsync(Guid id, CancellationToken ct = default)
        {
            var componentExists = await context.Components
                .AnyAsync(c => c.Id == id, ct);

            if (!componentExists)
                return ComponentErrors.NotFound;

            var componentInUse = await context.SupportRequestComponent
                .AnyAsync(src => src.ComponentId == id, ct);

            if (componentInUse)
                return ComponentErrors.InUse;

            try
            {
                var deletedCount = await context.Components
                    .Where(c => c.Id == id && c.Quantity == 0)
                    .ExecuteDeleteAsync(ct);

                if(deletedCount == 0)
                {
                    var stillExists = await context.Components
                        .AnyAsync(c => c.Id == id, ct);

                    if (!stillExists)
                        return ComponentErrors.NotFound;

                    return ComponentErrors.NonZeroStock;
                }
                return Result.Success;
            }
            catch (PostgresException ex) when (
            (ex.SqlState == PostgresErrorCodes.RestrictViolation ||
            ex.SqlState == PostgresErrorCodes.ForeignKeyViolation) &&
            (ex.ConstraintName == "fk_support_request_component_components_component_id"))
            {
                return ComponentErrors.InUse;
            }
        }
    }
}
