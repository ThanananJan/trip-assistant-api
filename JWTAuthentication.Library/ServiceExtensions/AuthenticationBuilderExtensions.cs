using JWTAuthentication.Library.Model.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Library.ServiceExtensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static WebApplicationBuilder AddAuthenticationBuilders(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("Aws"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            return builder;
        }
    }
}