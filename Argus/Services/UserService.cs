using Argus.Data;
using Argus.Dtos.Users;
using Argus.Models;
using Argus.Services.Interfaces;
using Argus.Constants.Errors;
using AutoMapper;
using ErrorOr;
using Argus.Enums;
using BCrypt.Net;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Argus.Providers.Interfaces;

namespace Argus.Services
{
    public class UserService(AppDbContext context, IMapper mapper, ICurrentUserProvider currentUser) : IUserService
    {
        public async Task<ErrorOr<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await context.Users
                .AsNoTracking() 
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return users;
        }
        public async Task<ErrorOr<UserDto>> GetUserByIdAsync(Guid id)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .ProjectTo<UserDto?>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (user == null)
                return UserErrors.NotFound;

            return user;
        }
        public async Task<ErrorOr<UserDto>> GetUserByNameAsync(string userName)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(u => u.UserName == userName)
                .ProjectTo<UserDto?>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            
            if (user == null) 
                return UserErrors.NotFound;

            return user;
        }
        public async Task<ErrorOr<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            var isUserExists = await context.Users
                //Protection from dublicates. Check if the UserName or Name is busy
                .AnyAsync(u=>u.UserName == dto.UserName || u.Email == dto.Email);

            if (isUserExists)
                return UserErrors.AlreadyExists;
            //Mapping: Turning the DTO into a User model.

            var newUser = mapper.Map<User>(dto);

            //Password Hashing(BCrypt Magic)
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            newUser.Department = string.IsNullOrWhiteSpace(dto.Department)
                ? "Отдел не указан"
                : dto.Department;

            //Saving to the Database
            context.Users.Add(newUser);

            await context.SaveChangesAsync();

            //Result
            return mapper.Map<UserDto>(newUser);
        }
        public async Task<ErrorOr<Success>> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordDto dto)
        {
            if (id != currentUser.UserId && currentUser.Role != UserRole.Admin)
                return UserErrors.Forbidden;

            var user = await context.Users.FindAsync(id);

            if (user is null)
                return UserErrors.NotFound;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

            if (!isPasswordValid)
                return UserErrors.WrongCurrentPassword;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);


            await context.SaveChangesAsync();

            return Result.Success;
        }

        //Не забудь добавить проврку прав доступа в UpdateUserProfileAsync, чтобы обычный пользователь не мог менять чужой профиль.
        public async Task<ErrorOr<Success>> UpdateUserProfileAsync(Guid id, UpdateUserProfileDto dto)
        {

            if (id != currentUser.UserId && currentUser.Role != UserRole.Admin)
                return UserErrors.Forbidden;

            var user = await context.Users.FindAsync(id);

            if (user is null)
                return UserErrors.NotFound;

            mapper.Map(dto, user);

            await context.SaveChangesAsync();

            return Result.Success;
        }

        public async Task<ErrorOr<UserDto>> ValidateCredentialAsync(string userName, string password)
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user is null)
                return UserErrors.InvalidAuthentication;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
                return UserErrors.InvalidAuthentication;

            return mapper.Map<UserDto>(user);
        }

        public async Task<ErrorOr<Success>> DeleteUserAsync(Guid id)
        {
            if (currentUser.Role != UserRole.Admin)
                return UserErrors.Forbidden;

            var user = await context.Users.FindAsync(id);

            if(user is null)
                return UserErrors.NotFound;

            context.Users.Remove(user);

            await context.SaveChangesAsync(); // При вызове этого метода сработает переопределенный метод

            return Result.Success;
        }


        /*public async Task<ErrorOr<List<UserDto>>> GetDeletedUsersAsync()
        {
            var deletedUsers = await _context.Users
                .IgnoreQueryFilters() // Взламываем глобальный фильтр!
                .Where(u => u.IsDeleted == true) // Ищем только удаленных
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return deletedUsers;
        }*/
    }
}
