using Argus.Dtos.Components;
using Argus.Dtos.Users;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Argus.Constants.RouteNames;
using NpgsqlTypes;
using Argus.Constants.Errors;
using ErrorOr;

namespace Argus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync()
        {
            var result = await userService.GetAllUsersAsync();
            return result.Match<ActionResult<List<UserDto>>>(
                user => Ok(user),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}", Name = UserRoutes.GetUserById)]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id)
        {
            var result = await userService.GetUserByIdAsync(id);
            return result.Match<ActionResult<UserDto>>(
                user => Ok(user),
                errors => Problem(errors));
        }

        [HttpGet("by-name/{userName}", Name = UserRoutes.GetUserByName)]
        public async Task<ActionResult<UserDto>> GetUserByNameAsync(string userName)
        {
            var result = await userService.GetUserByNameAsync(userName);
            return result.Match<ActionResult<UserDto>>(
                user => Ok(user),
                errors => Problem(errors));
            //return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            var result = await userService.CreateUserAsync(dto);

            return result.Match<ActionResult<UserDto>>(
                user => CreatedAtRoute(
                    UserRoutes.GetUserById,
                    new { id = user.Id }, user),
                errors => Problem(errors));
        }
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateUserPasswordAsync(Guid id, [FromBody] UpdateUserPasswordDto dto)
        {
            var result = await userService.UpdateUserPasswordAsync(id, dto);

            // Используем .Match(). Успех превращаем в 204 NoContent, ошибки летят в наш базовый ApiController
            return result.Match(
                success => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
