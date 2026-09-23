using Argus.Constants.Errors;
using Argus.Constants.RouteNames;
using Argus.Constants.Security;
using Argus.Dtos.Components;
using Argus.Dtos.Users;
using Argus.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NpgsqlTypes;

namespace Argus.Controllers
{
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync(CancellationToken ct)
        {
            var result = await userService.GetAllUsersAsync(ct);
            return result.Match<ActionResult<List<UserDto>>>(
                user => Ok(user),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}", Name = UserRoutes.GetUserById)]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await userService.GetUserByIdAsync(id, ct);
            return result.Match<ActionResult<UserDto>>(
                user => Ok(user),
                errors => Problem(errors));
        }

        [HttpGet("by-name/{userName}", Name = UserRoutes.GetUserByName)]
        public async Task<ActionResult<UserDto>> GetUserByNameAsync(string userName, CancellationToken ct)
        {
            var result = await userService.GetUserByNameAsync(userName, ct);
            return result.Match<ActionResult<UserDto>>(
                user => Ok(user),
                errors => Problem(errors));
        }
        [AllowAnonymous]
        [EnableRateLimiting(RateLimitPolicies.Auth)]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto dto, CancellationToken ct)
        {
            var result = await userService.CreateUserAsync(dto, ct);

            return result.Match<ActionResult<UserDto>>(
                user => CreatedAtRoute(
                    UserRoutes.GetUserById,
                    new { id = user.Id }, user),
                errors => Problem(errors));
        }

        [HttpPatch("{id:guid}/password")]
        public async Task<IActionResult> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordDto dto, CancellationToken ct)
        {
            var result = await userService.UpdateUserPasswordAsync(id, dto, ct);

            // We use .Match(). We turn success into 204 NoContent, and errors are sent to our base ApiController.
            return result.Match(
                success => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpPatch("{id:guid}/profile")]
        public async Task<IActionResult> UpdateUserProfileAsync(Guid id, UpdateUserProfileDto dto, CancellationToken ct)
        {
            var result = await userService.UpdateUserProfileAsync(id, dto,ct);
            return result.Match(
                success => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken ct)
        {
            var result = await userService.DeleteUserAsync(id, ct);
            return result.Match(
                success => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}