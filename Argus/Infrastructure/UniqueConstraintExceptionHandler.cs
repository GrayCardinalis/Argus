using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql;

namespace Argus.Infrastructure
{
    public class UniqueConstraintExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // 1. Guard Clauses: Checking the type of exception and the Postgres error code
            if(exception is not DbUpdateException { InnerException: PostgresException pg})
                return false;

            if (pg.SqlState != PostgresErrorCodes.UniqueViolation)
                return false;
            // 2. Guard Clause: If the headers have already gone into the socket, we cannot generate ProblemDetails
            if (httpContext.Response.HasStarted)
                return false;
            // 3. Setting the response status
            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

            // 4. We use the native IProblemDetailsService to comply with the Content-Type: application/problem+json
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = "A record with the specified unique key already exists.",
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10"
                }
            });
        }
    }
}
