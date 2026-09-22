using Argus.Enums;
using Argus.Providers.Interfaces;

namespace Argus.Providers
{
    public class JwtCurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
    {
        public Guid? UserId
        {
            get
            {
                var value = httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
                
                return Guid.TryParse(value, out var userId)
                    ? userId 
                    : null;
            }
        }

        public UserRole? Role 
        {
            get
            {
                var value = httpContextAccessor.HttpContext?.User.FindFirst("role")?.Value;

                return Enum.TryParse<UserRole>(value, out var role) && Enum.IsDefined(role)
                    ? role
                    : null;
            }
        }
    }
}
