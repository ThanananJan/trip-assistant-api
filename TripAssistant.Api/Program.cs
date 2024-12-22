using JWTAuthentication.Library.ServiceExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TripAssistant.Library.Model.Utilities;
using TripAssistant.Api.Middlewares;
using TripAssistant.Api.ServiceExtensions;


var builder = WebApplication.CreateBuilder(args);
builder.AddAuthenticationBuilders();
// Add services to the container.
builder.Services
      .AddControllers()
      .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.DateFormatString = Utility.DateFormat["UTC"];
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
    });


builder.Services.AddApplicationServices();
builder.Services.AddJWTAuthenticationServices();
builder.Services.AddSwaggerServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseRouting();
app.UseCors(options => options.WithOrigins("*")
                              .AllowAnyMethod()
                              .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
