using Argus.Models;

namespace Argus.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
    }
}
