using JWTAuthentication.Library.Model.DB;
using JWTAuthentication.Library.Model.Dto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JWTAuthentication.Library.Services
{
    internal class TokenService
    {
        private readonly JwtSettings settings;
        public TokenService(IOptions<JwtSettings> options)
        {
            settings = options.Value;

        }
        private readonly string securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
        internal string GenerateToken(User user)
        {
            var tokenDescritpor = GetTokenDescriptor(user, settings);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler
                        .CreateToken(tokenDescritpor);
            return tokenHandler.WriteToken(token);

        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = GetSecretEncryptKey();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler
              .ValidateToken(token, tokenValidationParameters,
              out SecurityToken securityToken);
            JwtSecurityToken? jwtSecurityToken =
              securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null
              || !jwtSecurityToken.Header
                  .Alg
                  .Equals(SecurityAlgorithms.HmacSha256,
                  StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private byte[] GetSecretEncryptKey()
        {
            return Encoding.UTF8.GetBytes(settings.SecretKey);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(User user, JwtSettings settings)
        {
            var key = new SymmetricSecurityKey(GetSecretEncryptKey());
            var claims = GetClaim(user);
            var credential = new SigningCredentials(key, securityAlgorithm);
            var dtmExpires = DateTime
              .UtcNow
              .AddMinutes(Convert.ToInt32(settings.JwtTokenExpiredMinute));
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = dtmExpires,
                SigningCredentials = credential,
                Audience = settings.Audience,
                Issuer = settings.Issuer
            };
        }

        private List<Claim> GetClaim(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Name,
              JsonSerializer
              .Serialize(new JwtUserInfo()
              {
                  IdUser = user.IdUser,
                  NamUser = user.NamUser
              })));
            return claims;
        }


    }
}
