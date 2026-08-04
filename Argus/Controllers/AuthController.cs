using Argus.Services.Interfaces;
using Argus.Constants.RouteNames;
using Microsoft.AspNetCore.Mvc;
using Argus.Dtos.Authorization;
using ErrorOr;
using Argus.Dtos.Users;
using Microsoft.AspNetCore.RateLimiting;

namespace Argus.Controllers
{
    [EnableRateLimiting("login")]
    [Route("api/auth")]
    public class AuthController(IUserService userService) : ApiController
    {
        [HttpPost("login", Name = AuthRoutes.Login)]
        public async Task<ActionResult<UserDto>> LoginAsync(LoginRequestDto dto, CancellationToken ct)
        {
            var result = await userService.ValidateCredentialAsync(dto.UserName, dto.Password, ct);
            return result.Match<ActionResult<UserDto>>(
                success => Ok(success),
                errors => Problem(errors)
            );
        }
    }
}
