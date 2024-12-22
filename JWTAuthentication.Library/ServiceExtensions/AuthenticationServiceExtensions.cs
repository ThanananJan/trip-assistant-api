using JWTAuthentication.Library.Handlers;
using JWTAuthentication.Library.Helpers;
using JWTAuthentication.Library.Model.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JWTAuthentication.Library.Interfaces;
using JWTAuthentication.Library.Services;
using Microsoft.AspNetCore.Http;



namespace JWTAuthentication.Library.ServiceExtensions
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddJWTAuthenticationServices(this IServiceCollection services)
        {
            var options = services
                                .BuildServiceProvider()
                                .GetService<IOptions<JwtSettings>>();
            var tokenSettings = options == null ? new JwtSettings() : options.Value;
            services
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(optoins =>
              {
                  optoins.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSettings.SecretKey)),
                      ValidIssuer = tokenSettings.Issuer,
                      ValidAudience = tokenSettings.Audience,
                      ClockSkew = TimeSpan.Zero
                  };
              });
            services.AddHttpContextAccessor();
            services.AddScoped<AWSAuthenticationHelper>();
            services.AddScoped<AWSAuthorizationFilter>();
            services.AddScoped<IJwtAuthenticationHelper, JwtAuthenticationHelper>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
