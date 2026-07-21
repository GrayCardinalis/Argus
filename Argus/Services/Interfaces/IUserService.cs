using Argus.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<List<UserDto>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<ErrorOr<UserDto>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ErrorOr<UserDto>> GetUserByNameAsync(string userName, CancellationToken cancellationToken = default);
        // 2. Authentication (For future login)
        Task<ErrorOr<UserDto>> ValidateCredentialAsync(string userName, string password, CancellationToken cancellationToken = default);
        Task<ErrorOr<UserDto>> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
        Task<ErrorOr<Success>> UpdateUserProfileAsync(Guid id, UpdateUserProfileDto dto, CancellationToken cancellationToken = default);
        Task<ErrorOr<Success>> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordDto dto, CancellationToken cancellationToken = default);
        //Deletion, change to deactivation in the future
        Task<ErrorOr<Success>> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
        /*Task<ErrorOr<List<UserDto>>> GetDeletedUsersAsync();*/
    }
}
