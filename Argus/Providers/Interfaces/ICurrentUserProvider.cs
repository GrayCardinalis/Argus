using Argus.Enums;

namespace Argus.Providers.Interfaces
{
    public interface ICurrentUserProvider
    {
        Guid? UserId{ get; }
        UserRole? Role{ get; }
    }
}
