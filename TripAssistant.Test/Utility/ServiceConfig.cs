


using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using TripAssistant.Library;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Services;

namespace TripAssistant.Test.Utility
{
    internal class ServiceConfig
    {
        internal static DbContextOptions<TripAssistantDbContext> ContextOptions = new DbContextOptionsBuilder<TripAssistantDbContext>()
                                                                           .Options;
        internal static IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles())));
        internal static ILogger<TripService> Logger = new Mock<ILogger<TripService>>().Object;
    }
    public static class ModelStateExtension
    {
        public static void AddValidationErrors(this ModelStateDictionary modelState, object model)
        {
            var context = new ValidationContext(model, null, null);

            var results = new List<ValidationResult>();

            Validator.TryValidateObject(model, context, results, true);
            foreach (var result in results)
            {
                var name = result.MemberNames.First();
                modelState.AddModelError(name, result.ErrorMessage ?? string.Empty);
            }
        }
    }
    internal class MockDatabaseFacade : DatabaseFacade
    {
        public MockDatabaseFacade(DbContext context) : base(context)
        {
        }
        public override IDbContextTransaction BeginTransaction() =>
           Mock.Of<IDbContextTransaction>();
        public override Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(Mock.Of<IDbContextTransaction>());
    }
}
