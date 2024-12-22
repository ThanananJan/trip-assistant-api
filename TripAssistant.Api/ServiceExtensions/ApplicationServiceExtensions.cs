using JWTAuthentication.Library.Helpers;
using JWTAuthentication.Library.Model.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.IO;
using TripAssistant.Library;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Services;

namespace TripAssistant.Api.ServiceExtensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var dbConnection = config?.GetConnectionString("TripAssistantConnectionString");
            Console.WriteLine(dbConnection);
            var version = new MySqlServerVersion(new Version(9, 1, 0));
            services.AddDbContext<TripAssistantDbContext>(options =>
            {
                options.UseMySql(dbConnection,
            version,
            s => s.MigrationsAssembly("TripAssistant.Api"))
          .LogTo(Console.WriteLine, LogLevel.Information)
          .EnableDetailedErrors()
          .ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });
            services.AddDbContext<JwtAuthDbContext>(options =>
            {
                options.UseMySql(dbConnection,
            version,
            s => s.MigrationsAssembly("TripAssistant.Api"))
          .LogTo(Console.WriteLine, LogLevel.Information)
          .EnableDetailedErrors()
          .ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            });
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IJwtAuthenticationHelper, JwtAuthenticationHelper>();
            return services;
        }
    }
}
