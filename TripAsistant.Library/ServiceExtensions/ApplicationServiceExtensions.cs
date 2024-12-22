
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;
using TripAssistant.Library.Model.DB;



namespace TripAssistant.Library.ServiceExtensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddTripAssistantAppServices(this IServiceCollection services)
        {
            var path = "D:\\";
            var dbPath = System.IO.Path.Join(path, "blogging.db");
            services.AddDbContext<TripAssistantDbContext>(options =>
            {
                options.UseSqlite($"Data Source={dbPath}");
            });
            return services;
        }
    }
}
