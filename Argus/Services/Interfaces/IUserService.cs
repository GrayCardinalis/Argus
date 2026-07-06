using Argus.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<List<UserDto>>> GetAllUsersAsync();
        Task<ErrorOr<UserDto?>> GetUserByIdAsync(Guid id);
        Task<ErrorOr<UserDto?>> GetUserByNameAsync(string userName);
        // 2. Authentication (For future login)
        Task<UserDto?> ValidateCredentialAsync(string userName, string password);
        Task<ErrorOr<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<bool> UpdateUserProfileAsync(Guid id, UpdateUserProfileDto dto);
        Task<ErrorOr<Success>> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordDto dto);
        //Deletion, change to deactivation in the future
        Task<bool> DeleteUserAsync(Guid id);
    }
}
