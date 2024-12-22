using Microsoft.OpenApi.Models;

namespace TripAssistant.Api.ServiceExtensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Trip Assistent Service API",
                Version = "v1"
            });
                options.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
                Description = $"JWT Authorization header using the Bearer scheme.{Environment.NewLine} Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
              {{
          new OpenApiSecurityScheme
          {
            Reference =new OpenApiReference{
              Type = ReferenceType.SecurityScheme,
              Id="Bearer"
            }
          },
        Array.Empty<string>()
        }
            });
            });
            return services;
        }
        public static WebApplication AddUseSwaggers(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
