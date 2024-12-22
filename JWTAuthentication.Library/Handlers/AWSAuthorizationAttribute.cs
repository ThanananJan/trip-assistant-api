using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using JWTAuthentication.Library.Model.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace JWTAuthentication.Library.Handlers
{
    public class AWSAuthorizationAttribute : ServiceFilterAttribute
    {
        public AWSAuthorizationAttribute() : base(typeof(AWSAuthorizationFilter))
        {
        }
    }
    public class AWSAuthorizationFilter(IOptions<AwsSettings> options) : IAuthorizationFilter
    {
        private string validateIssuer = $"https://cognito-idp.{options?.Value.CognitoRegion}.amazonaws.com/{options?.Value.CognitoUserPoolId}";
    private string jwksEndpoint = $"https://cognito-idp.{options?.Value.CognitoRegion}.amazonaws.com/{options?.Value.CognitoUserPoolId}/.well-known/openid-configuration";
    public void OnAuthorization(AuthorizationFilterContext context)
    {

        var authorizationHeader = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
        if (authorizationHeader == null)
        {
            Unauthorized(context); return;
        }

        try
        {
            var token = authorizationHeader.Replace(JwtBearerDefaults.AuthenticationScheme, "").Trim();
            var task = ValidateTokenAsync(token);
            task.Wait();
            var claimsIdentity = task.Result;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                Unauthorized(context); return;
            }
            context.HttpContext.Items["ClaimsIdentity"] = claimsIdentity;
        }
        catch
        {
            throw;
        }

        return;
    }

    private async Task<OpenIdConnectConfiguration?> GetOpenIdConfigurationAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(jwksEndpoint);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<OpenIdConnectConfiguration>(jsonString);
    }
    public async Task<ClaimsIdentity> ValidateTokenAsync(string token)
    {
        var openIdConfig = await GetOpenIdConfigurationAsync();
        var jwksUri = openIdConfig?.JwksUri ?? jwksEndpoint; // Extract JWK endpoint

        var signingKey = await GetSigningKeyAsync(token, jwksUri);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false, // Not always present in Cognito tokens
            ValidateLifetime = true,
            ValidIssuer = validateIssuer,
            ClockSkew = TimeSpan.Zero, // Optional: Adjust if needed
            IssuerSigningKey = signingKey
        };

        var tokenHandler = new JsonWebTokenHandler();
        var principal = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        return principal.ClaimsIdentity;
    }
    private static async Task<SecurityKey> GetSigningKeyAsync(string token, string jwksUri)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJsonWebToken(token);
        var kid = jwtSecurityToken.Kid;

        using var client = new HttpClient();
        var response = await client.GetAsync(jwksUri);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var jwks = System.Text.Json.JsonSerializer.Deserialize<JsonWebKeySet>(jsonString); // Use System.Text.Json
        var key = (jwks?.Keys.FirstOrDefault(k => k.Kid == kid)) ?? throw new Exception("Invalid JWT: Signing key not found");

        var parameters = new RSAParameters
        {
            Modulus = Base64UrlEncoder.DecodeBytes(key.N),
            Exponent = Base64UrlEncoder.DecodeBytes(key.E),
        };

        var rsa = RSA.Create();
        rsa.ImportParameters(parameters);
        return new RsaSecurityKey(rsa);
    }

    private void Unauthorized(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedResult();
    }

}
}
