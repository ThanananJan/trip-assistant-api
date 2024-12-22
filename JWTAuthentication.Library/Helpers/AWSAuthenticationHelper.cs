using JWTAuthentication.Library.Model.DB;
using JWTAuthentication.Library.Model.Dto;
using JWTAuthentication.Library.Model.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace JWTAuthentication.Library.Helpers
{
    public class AWSAuthenticationHelper(IServiceScopeFactory scopeFactory, IOptions<AwsSettings> options)
    {
        public async Task<AWSUserInfo?> GetUserInfo()
    {
        try
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var token = _context?.HttpContext?.Request.Headers[RequestHeader.Authorization].ToString();
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme,
                                                                                            token?.Replace(JwtBearerDefaults.AuthenticationScheme, "").Trim());
                var response = await client.GetAsync(options?.Value.CognitoUserInfoUrl);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<AWSUserInfo>(jsonString);
                return userInfo;
            }
        }
        catch
        {
            return null;
        }
    }
}
public class AWSUserInfo
{
    public Guid Sub { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
}
