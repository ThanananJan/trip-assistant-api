using JWTAuthentication.Library.Helpers;
using JWTAuthentication.Library.Model.Dto;

namespace JWTAuthentication.Library.Interfaces
{
    public interface IAuthenticationService
    {
        public JwtToken GenerateToken(AWSUserInfo userInfo);
        public JwtToken GenerateRefreshToken(RefreshTokenRequest request);
    }
}
