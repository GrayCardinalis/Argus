using AutoMapper;
using Argus.Models;
using Argus.Dtos.Components;
using Argus.Dtos.Users;

namespace Argus.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                // Data creation: ignore the PasswordHash, we will fill it in ourselves in the UserService
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UpdateUserProfileDto, User>();
        }
    }
}
