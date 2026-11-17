using Argus.Dtos.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Argus.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PagedResultDto<TDto>> ToPagedResultAsync<TEntity, TDto>(this IQueryable<TEntity> query, PaginationQueryDto dto, global::AutoMapper.IConfigurationProvider mappingConfiguration, CancellationToken ct = default)
        {
            var totalCount = await query.CountAsync(ct);

            var result = new PagedResultDto<TDto>
            {
                TotalCount = totalCount,
                Page = dto.Page,
                PageSize = dto.PageSize
            };

            var skip = ((long)dto.Page - 1) * dto.PageSize;

            if (skip >= totalCount)
                return result;

            result.Items = await query
                .Skip((int)skip)
                .Take(dto.PageSize)
                .ProjectTo<TDto>(mappingConfiguration)
                .ToListAsync(ct);

            return result;
        }
    }
}