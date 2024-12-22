

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWTAuthentication.Library.Model.Dto;

namespace JWTAuthentication.Library.Helpers
{
    public interface IJwtAuthenticationHelper
    {
        JwtUserInfo? GetUserInfoByClaimsIdentity();
    }
    public class JwtAuthenticationHelper : IJwtAuthenticationHelper
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public JwtAuthenticationHelper(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public JwtUserInfo? GetUserInfoByClaimsIdentity()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                    var identity = context.HttpContext?.User.Identity as ClaimsIdentity;
                    var userInfoString = identity?.FindFirst(JwtRegisteredClaimNames.Name)?.Value ?? string.Empty;
                    var userInfo = JsonConvert
                      .DeserializeObject<JwtUserInfo>(userInfoString == null ?
                      string.Empty : userInfoString);
                    return userInfo;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
