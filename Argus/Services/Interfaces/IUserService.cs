using Argus.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Argus.Dtos.Authorization;

namespace Argus.Services.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<List<UserDto>>> GetAllUsersAsync(CancellationToken ct = default);
        Task<ErrorOr<UserDto>> GetUserByIdAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<UserDto>> GetUserByNameAsync(string userName, CancellationToken ct = default);
        // 2. Authentication (For future login)
        Task<ErrorOr<LoginResponseDto>> LoginAsync(string userName, string password, CancellationToken ct = default);
        Task<ErrorOr<UserDto>> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateUserProfileAsync(Guid id, UpdateUserProfileDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordDto dto, CancellationToken ct = default);
        //Deletion, change to deactivation in the future
        Task<ErrorOr<Success>> DeleteUserAsync(Guid id, CancellationToken ct = default);
        /*Task<ErrorOr<List<UserDto>>> GetDeletedUsersAsync();*/
    }
}
