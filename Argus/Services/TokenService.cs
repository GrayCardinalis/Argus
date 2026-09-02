using Argus.Models;
using Argus.Options;
using Argus.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Argus.Services
{
    public class TokenService(IOptions<JwtOptions> options):ITokenService
    {
        private readonly JwtOptions _options = options.Value;

        private readonly SigningCredentials _signingCredentials = new(
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(options.Value.Key)),
            SecurityAlgorithms.HmacSha256);

        private readonly JsonWebTokenHandler _handler = new();

        public string GenerateAccessToken(User user)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeInMinutes),
                SigningCredentials = _signingCredentials,
                Claims = new Dictionary<string, object>
                {
                    ["sub"] = user.Id.ToString(),
                    ["role"] = user.Role.ToString(),
                    ["jti"] = Guid.NewGuid().ToString()
                }
            };

            return _handler.CreateToken(descriptor);
        }
    }
}
