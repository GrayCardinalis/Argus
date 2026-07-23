using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql;

namespace Argus.Infrastructure
{
    public class UniqueConstraintExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DbUpdateException { InnerException: PostgresException pg })
                return false;

            if (pg.SqlState != PostgresErrorCodes.UniqueViolation)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = "A record with the specified unique key already exists",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
            };


            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            
            return true;

        }
    }
}
